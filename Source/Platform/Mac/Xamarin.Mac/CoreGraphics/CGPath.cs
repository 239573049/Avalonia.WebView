using System;
using System.Runtime.InteropServices;
using Foundation;
using ObjCRuntime;

namespace CoreGraphics;

public class CGPath : INativeObject, IDisposable
{
	public delegate void ApplierFunction(CGPathElement element);

	private delegate void CGPathApplierFunction(IntPtr info, IntPtr element);

	internal IntPtr handle;

	public IntPtr Handle => handle;

	public bool IsEmpty => CGPathIsEmpty(handle);

	public CGPoint CurrentPoint => CGPathGetCurrentPoint(handle);

	public CGRect BoundingBox => CGPathGetBoundingBox(handle);

	public CGRect PathBoundingBox => CGPathGetPathBoundingBox(handle);

	[DllImport("/System/Library/Frameworks/ApplicationServices.framework/Versions/A/Frameworks/CoreGraphics.framework/CoreGraphics")]
	private static extern IntPtr CGPathCreateMutable();

	public CGPath()
	{
		handle = CGPathCreateMutable();
	}

	public CGPath(CGPath reference, CGAffineTransform transform)
	{
		if (reference == null)
		{
			throw new ArgumentNullException("reference");
		}
		handle = CGPathCreateMutableCopyByTransformingPath(reference.Handle, ref transform);
	}

	[DllImport("/System/Library/Frameworks/ApplicationServices.framework/Versions/A/Frameworks/CoreGraphics.framework/CoreGraphics")]
	private static extern IntPtr CGPathCreateMutableCopy(IntPtr path);

	public CGPath(CGPath basePath)
	{
		if (basePath == null)
		{
			throw new ArgumentNullException("basePath");
		}
		handle = CGPathCreateMutableCopy(basePath.handle);
	}

	public CGPath(IntPtr handle)
	{
		CGPathRetain(handle);
		this.handle = handle;
	}

	[Preserve(Conditional = true)]
	internal CGPath(IntPtr handle, bool owns)
	{
		if (!owns)
		{
			CGPathRetain(handle);
		}
		this.handle = handle;
	}

	~CGPath()
	{
		Dispose(disposing: false);
	}

	public void Dispose()
	{
		Dispose(disposing: true);
		GC.SuppressFinalize(this);
	}

	[DllImport("/System/Library/Frameworks/ApplicationServices.framework/Versions/A/Frameworks/CoreGraphics.framework/CoreGraphics")]
	private static extern void CGPathRelease(IntPtr path);

	[DllImport("/System/Library/Frameworks/ApplicationServices.framework/Versions/A/Frameworks/CoreGraphics.framework/CoreGraphics")]
	private static extern IntPtr CGPathRetain(IntPtr path);

	protected virtual void Dispose(bool disposing)
	{
		if (handle != IntPtr.Zero)
		{
			CGPathRelease(handle);
			handle = IntPtr.Zero;
		}
	}

	[DllImport("/System/Library/Frameworks/ApplicationServices.framework/Versions/A/Frameworks/CoreGraphics.framework/CoreGraphics")]
	private static extern bool CGPathEqualToPath(IntPtr path1, IntPtr path2);

	public static bool operator ==(CGPath path1, CGPath path2)
	{
		return object.Equals(path1, path2);
	}

	public static bool operator !=(CGPath path1, CGPath path2)
	{
		return !object.Equals(path1, path2);
	}

	public override int GetHashCode()
	{
		return handle.GetHashCode();
	}

	public override bool Equals(object o)
	{
		CGPath cGPath = o as CGPath;
		if (cGPath == null)
		{
			return false;
		}
		return CGPathEqualToPath(handle, cGPath.handle);
	}

	[DllImport("/System/Library/Frameworks/ApplicationServices.framework/Versions/A/Frameworks/CoreGraphics.framework/CoreGraphics")]
	private unsafe static extern void CGPathMoveToPoint(IntPtr path, CGAffineTransform* m, nfloat x, nfloat y);

	public unsafe void MoveToPoint(nfloat x, nfloat y)
	{
		CGPathMoveToPoint(handle, null, x, y);
	}

	public unsafe void MoveToPoint(CGPoint point)
	{
		CGPathMoveToPoint(handle, null, point.X, point.Y);
	}

	public unsafe void MoveToPoint(CGAffineTransform transform, nfloat x, nfloat y)
	{
		CGPathMoveToPoint(handle, &transform, x, y);
	}

	public unsafe void MoveToPoint(CGAffineTransform transform, CGPoint point)
	{
		CGPathMoveToPoint(handle, &transform, point.X, point.Y);
	}

	[DllImport("/System/Library/Frameworks/ApplicationServices.framework/Versions/A/Frameworks/CoreGraphics.framework/CoreGraphics")]
	private unsafe static extern void CGPathAddLineToPoint(IntPtr path, CGAffineTransform* m, nfloat x, nfloat y);

	public unsafe void AddLineToPoint(nfloat x, nfloat y)
	{
		CGPathAddLineToPoint(handle, null, x, y);
	}

	public unsafe void AddLineToPoint(CGPoint point)
	{
		CGPathAddLineToPoint(handle, null, point.X, point.Y);
	}

	public unsafe void AddLineToPoint(CGAffineTransform transform, nfloat x, nfloat y)
	{
		CGPathAddLineToPoint(handle, &transform, x, y);
	}

	public unsafe void AddLineToPoint(CGAffineTransform transform, CGPoint point)
	{
		CGPathAddLineToPoint(handle, &transform, point.X, point.Y);
	}

	[DllImport("/System/Library/Frameworks/ApplicationServices.framework/Versions/A/Frameworks/CoreGraphics.framework/CoreGraphics")]
	private unsafe static extern void CGPathAddQuadCurveToPoint(IntPtr path, CGAffineTransform* m, nfloat cpx, nfloat cpy, nfloat x, nfloat y);

	public unsafe void AddQuadCurveToPoint(nfloat cpx, nfloat cpy, nfloat x, nfloat y)
	{
		CGPathAddQuadCurveToPoint(handle, null, cpx, cpy, x, y);
	}

	public unsafe void AddQuadCurveToPoint(CGAffineTransform transform, nfloat cpx, nfloat cpy, nfloat x, nfloat y)
	{
		CGPathAddQuadCurveToPoint(handle, &transform, cpx, cpy, x, y);
	}

	[DllImport("/System/Library/Frameworks/ApplicationServices.framework/Versions/A/Frameworks/CoreGraphics.framework/CoreGraphics")]
	private unsafe static extern void CGPathAddCurveToPoint(IntPtr path, CGAffineTransform* m, nfloat cp1x, nfloat cp1y, nfloat cp2x, nfloat cp2y, nfloat x, nfloat y);

	public unsafe void AddCurveToPoint(CGAffineTransform transform, nfloat cp1x, nfloat cp1y, nfloat cp2x, nfloat cp2y, nfloat x, nfloat y)
	{
		CGPathAddCurveToPoint(handle, &transform, cp1x, cp1y, cp2x, cp2y, x, y);
	}

	public unsafe void AddCurveToPoint(CGAffineTransform transform, CGPoint cp1, CGPoint cp2, CGPoint point)
	{
		CGPathAddCurveToPoint(handle, &transform, cp1.X, cp1.Y, cp2.X, cp2.Y, point.X, point.Y);
	}

	public unsafe void AddCurveToPoint(nfloat cp1x, nfloat cp1y, nfloat cp2x, nfloat cp2y, nfloat x, nfloat y)
	{
		CGPathAddCurveToPoint(handle, null, cp1x, cp1y, cp2x, cp2y, x, y);
	}

	public unsafe void AddCurveToPoint(CGPoint cp1, CGPoint cp2, CGPoint point)
	{
		CGPathAddCurveToPoint(handle, null, cp1.X, cp1.Y, cp2.X, cp2.Y, point.X, point.Y);
	}

	[DllImport("/System/Library/Frameworks/ApplicationServices.framework/Versions/A/Frameworks/CoreGraphics.framework/CoreGraphics")]
	private static extern void CGPathCloseSubpath(IntPtr path);

	public void CloseSubpath()
	{
		CGPathCloseSubpath(handle);
	}

	[DllImport("/System/Library/Frameworks/ApplicationServices.framework/Versions/A/Frameworks/CoreGraphics.framework/CoreGraphics")]
	private unsafe static extern void CGPathAddRect(IntPtr path, CGAffineTransform* m, CGRect rect);

	public unsafe void AddRect(CGAffineTransform transform, CGRect rect)
	{
		CGPathAddRect(handle, &transform, rect);
	}

	public unsafe void AddRect(CGRect rect)
	{
		CGPathAddRect(handle, null, rect);
	}

	[DllImport("/System/Library/Frameworks/ApplicationServices.framework/Versions/A/Frameworks/CoreGraphics.framework/CoreGraphics")]
	private unsafe static extern void CGPathAddRects(IntPtr path, CGAffineTransform* m, CGRect[] rects, nint count);

	public unsafe void AddRects(CGAffineTransform m, CGRect[] rects)
	{
		if (rects == null)
		{
			throw new ArgumentNullException("rects");
		}
		CGPathAddRects(handle, &m, rects, rects.Length);
	}

	public unsafe void AddRects(CGAffineTransform m, CGRect[] rects, int count)
	{
		if (rects == null)
		{
			throw new ArgumentNullException("rects");
		}
		if (count > rects.Length)
		{
			throw new ArgumentException("count");
		}
		CGPathAddRects(handle, &m, rects, count);
	}

	public unsafe void AddRects(CGRect[] rects)
	{
		if (rects == null)
		{
			throw new ArgumentNullException("rects");
		}
		CGPathAddRects(handle, null, rects, rects.Length);
	}

	public unsafe void AddRects(CGRect[] rects, int count)
	{
		if (rects == null)
		{
			throw new ArgumentNullException("rects");
		}
		if (count > rects.Length)
		{
			throw new ArgumentException("count");
		}
		CGPathAddRects(handle, null, rects, count);
	}

	[DllImport("/System/Library/Frameworks/ApplicationServices.framework/Versions/A/Frameworks/CoreGraphics.framework/CoreGraphics")]
	private unsafe static extern void CGPathAddLines(IntPtr path, CGAffineTransform* m, CGPoint[] points, nint count);

	public unsafe void AddLines(CGAffineTransform m, CGPoint[] points)
	{
		if (points == null)
		{
			throw new ArgumentNullException("points");
		}
		CGPathAddLines(handle, &m, points, points.Length);
	}

	public unsafe void AddLines(CGAffineTransform m, CGPoint[] points, int count)
	{
		if (points == null)
		{
			throw new ArgumentNullException("points");
		}
		if (count > points.Length)
		{
			throw new ArgumentException("count");
		}
		CGPathAddLines(handle, &m, points, count);
	}

	public unsafe void AddLines(CGPoint[] points)
	{
		if (points == null)
		{
			throw new ArgumentNullException("points");
		}
		CGPathAddLines(handle, null, points, points.Length);
	}

	public unsafe void AddLines(CGPoint[] points, int count)
	{
		if (points == null)
		{
			throw new ArgumentNullException("points");
		}
		if (count > points.Length)
		{
			throw new ArgumentException("count");
		}
		CGPathAddLines(handle, null, points, count);
	}

	[DllImport("/System/Library/Frameworks/ApplicationServices.framework/Versions/A/Frameworks/CoreGraphics.framework/CoreGraphics")]
	private unsafe static extern void CGPathAddEllipseInRect(IntPtr path, CGAffineTransform* m, CGRect rect);

	public unsafe void AddEllipseInRect(CGAffineTransform m, CGRect rect)
	{
		CGPathAddEllipseInRect(handle, &m, rect);
	}

	public unsafe void AddEllipseInRect(CGRect rect)
	{
		CGPathAddEllipseInRect(handle, null, rect);
	}

	[DllImport("/System/Library/Frameworks/ApplicationServices.framework/Versions/A/Frameworks/CoreGraphics.framework/CoreGraphics")]
	private unsafe static extern void CGPathAddArc(IntPtr path, CGAffineTransform* m, nfloat x, nfloat y, nfloat radius, nfloat startAngle, nfloat endAngle, bool clockwise);

	public unsafe void AddArc(CGAffineTransform m, nfloat x, nfloat y, nfloat radius, nfloat startAngle, nfloat endAngle, bool clockwise)
	{
		CGPathAddArc(handle, &m, x, y, radius, startAngle, endAngle, clockwise);
	}

	public unsafe void AddArc(nfloat x, nfloat y, nfloat radius, nfloat startAngle, nfloat endAngle, bool clockwise)
	{
		CGPathAddArc(handle, null, x, y, radius, startAngle, endAngle, clockwise);
	}

	[DllImport("/System/Library/Frameworks/ApplicationServices.framework/Versions/A/Frameworks/CoreGraphics.framework/CoreGraphics")]
	private unsafe static extern void CGPathAddArcToPoint(IntPtr path, CGAffineTransform* m, nfloat x1, nfloat y1, nfloat x2, nfloat y2, nfloat radius);

	public unsafe void AddArcToPoint(CGAffineTransform m, nfloat x1, nfloat y1, nfloat x2, nfloat y2, nfloat radius)
	{
		CGPathAddArcToPoint(handle, &m, x1, y1, x2, y2, radius);
	}

	public unsafe void AddArcToPoint(nfloat x1, nfloat y1, nfloat x2, nfloat y2, nfloat radius)
	{
		CGPathAddArcToPoint(handle, null, x1, y1, x2, y2, radius);
	}

	[DllImport("/System/Library/Frameworks/ApplicationServices.framework/Versions/A/Frameworks/CoreGraphics.framework/CoreGraphics")]
	private unsafe static extern void CGPathAddRelativeArc(IntPtr path, CGAffineTransform* m, nfloat x, nfloat y, nfloat radius, nfloat startAngle, nfloat delta);

	public unsafe void AddRelativeArc(CGAffineTransform m, nfloat x, nfloat y, nfloat radius, nfloat startAngle, nfloat delta)
	{
		CGPathAddRelativeArc(handle, &m, x, y, radius, startAngle, delta);
	}

	public unsafe void AddRelativeArc(nfloat x, nfloat y, nfloat radius, nfloat startAngle, nfloat delta)
	{
		CGPathAddRelativeArc(handle, null, x, y, radius, startAngle, delta);
	}

	[DllImport("/System/Library/Frameworks/ApplicationServices.framework/Versions/A/Frameworks/CoreGraphics.framework/CoreGraphics")]
	private unsafe static extern void CGPathAddPath(IntPtr path1, CGAffineTransform* m, IntPtr path2);

	public unsafe void AddPath(CGAffineTransform t, CGPath path2)
	{
		if (path2 == null)
		{
			throw new ArgumentNullException("path2");
		}
		CGPathAddPath(handle, &t, path2.handle);
	}

	public unsafe void AddPath(CGPath path2)
	{
		if (path2 == null)
		{
			throw new ArgumentNullException("path2");
		}
		CGPathAddPath(handle, null, path2.handle);
	}

	[DllImport("/System/Library/Frameworks/ApplicationServices.framework/Versions/A/Frameworks/CoreGraphics.framework/CoreGraphics")]
	private static extern bool CGPathIsEmpty(IntPtr path);

	[DllImport("/System/Library/Frameworks/ApplicationServices.framework/Versions/A/Frameworks/CoreGraphics.framework/CoreGraphics")]
	private static extern bool CGPathIsRect(IntPtr path, out CGRect rect);

	public bool IsRect(out CGRect rect)
	{
		return CGPathIsRect(handle, out rect);
	}

	[DllImport("/System/Library/Frameworks/ApplicationServices.framework/Versions/A/Frameworks/CoreGraphics.framework/CoreGraphics")]
	private static extern CGPoint CGPathGetCurrentPoint(IntPtr path);

	[DllImport("/System/Library/Frameworks/ApplicationServices.framework/Versions/A/Frameworks/CoreGraphics.framework/CoreGraphics")]
	private static extern CGRect CGPathGetBoundingBox(IntPtr path);

	[DllImport("/System/Library/Frameworks/ApplicationServices.framework/Versions/A/Frameworks/CoreGraphics.framework/CoreGraphics")]
	private static extern CGRect CGPathGetPathBoundingBox(IntPtr path);

	[DllImport("/System/Library/Frameworks/ApplicationServices.framework/Versions/A/Frameworks/CoreGraphics.framework/CoreGraphics")]
	private unsafe static extern bool CGPathContainsPoint(IntPtr path, CGAffineTransform* m, CGPoint point, bool eoFill);

	public unsafe bool ContainsPoint(CGAffineTransform m, CGPoint point, bool eoFill)
	{
		return CGPathContainsPoint(handle, &m, point, eoFill);
	}

	public unsafe bool ContainsPoint(CGPoint point, bool eoFill)
	{
		return CGPathContainsPoint(handle, null, point, eoFill);
	}

	private static void ApplierCallback(IntPtr info, IntPtr element_ptr)
	{
		GCHandle gCHandle = GCHandle.FromIntPtr(info);
		CGPathElement element = new CGPathElement(Marshal.ReadInt32(element_ptr, 0));
		ApplierFunction applierFunction = (ApplierFunction)gCHandle.Target;
		IntPtr intPtr = Marshal.ReadIntPtr(element_ptr, IntPtr.Size);
		int num = Marshal.SizeOf(typeof(CGPoint));
		switch (element.Type)
		{
		case CGPathElementType.MoveToPoint:
		case CGPathElementType.AddLineToPoint:
			element.Point1 = (CGPoint)Marshal.PtrToStructure(intPtr, typeof(CGPoint));
			break;
		case CGPathElementType.AddQuadCurveToPoint:
			element.Point1 = (CGPoint)Marshal.PtrToStructure(intPtr, typeof(CGPoint));
			element.Point2 = (CGPoint)Marshal.PtrToStructure((IntPtr)((long)intPtr + num), typeof(CGPoint));
			break;
		case CGPathElementType.AddCurveToPoint:
			element.Point1 = (CGPoint)Marshal.PtrToStructure(intPtr, typeof(CGPoint));
			element.Point2 = (CGPoint)Marshal.PtrToStructure((IntPtr)((long)intPtr + num), typeof(CGPoint));
			element.Point3 = (CGPoint)Marshal.PtrToStructure((IntPtr)((long)intPtr + 2 * num), typeof(CGPoint));
			break;
		}
		applierFunction(element);
	}

	[DllImport("/System/Library/Frameworks/ApplicationServices.framework/Versions/A/Frameworks/CoreGraphics.framework/CoreGraphics")]
	private static extern void CGPathApply(IntPtr path, IntPtr info, CGPathApplierFunction function);

	public void Apply(ApplierFunction func)
	{
		GCHandle value = GCHandle.Alloc(func);
		CGPathApply(handle, GCHandle.ToIntPtr(value), ApplierCallback);
		value.Free();
	}

	private static CGPath MakeMutable(IntPtr source)
	{
		IntPtr intPtr = CGPathCreateMutableCopy(source);
		return new CGPath(intPtr, owns: true);
	}

	[DllImport("/System/Library/Frameworks/ApplicationServices.framework/Versions/A/Frameworks/CoreGraphics.framework/CoreGraphics")]
	private unsafe static extern IntPtr CGPathCreateCopyByDashingPath(IntPtr path, CGAffineTransform* transform, nfloat phase, nfloat[] lengths, nint count);

	public CGPath CopyByDashingPath(CGAffineTransform transform, nfloat[] lengths)
	{
		return CopyByDashingPath(transform, lengths, 0);
	}

	public unsafe CGPath CopyByDashingPath(CGAffineTransform transform, nfloat[] lengths, nfloat phase)
	{
		return MakeMutable(CGPathCreateCopyByDashingPath(handle, &transform, phase, lengths, (lengths != null) ? lengths.Length : 0));
	}

	public CGPath CopyByDashingPath(nfloat[] lengths)
	{
		return CopyByDashingPath(lengths, 0);
	}

	public unsafe CGPath CopyByDashingPath(nfloat[] lengths, nfloat phase)
	{
		IntPtr source = CGPathCreateCopyByDashingPath(handle, null, phase, lengths, (lengths != null) ? lengths.Length : 0);
		return MakeMutable(source);
	}

	public CGPath Copy()
	{
		return MakeMutable(handle);
	}

	[DllImport("/System/Library/Frameworks/ApplicationServices.framework/Versions/A/Frameworks/CoreGraphics.framework/CoreGraphics")]
	private unsafe static extern IntPtr CGPathCreateCopyByStrokingPath(IntPtr path, CGAffineTransform* transform, nfloat lineWidth, CGLineCap lineCap, CGLineJoin lineJoin, nfloat miterLimit);

	public unsafe CGPath CopyByStrokingPath(CGAffineTransform transform, nfloat lineWidth, CGLineCap lineCap, CGLineJoin lineJoin, nfloat miterLimit)
	{
		return MakeMutable(CGPathCreateCopyByStrokingPath(handle, &transform, lineWidth, lineCap, lineJoin, miterLimit));
	}

	public unsafe CGPath CopyByStrokingPath(nfloat lineWidth, CGLineCap lineCap, CGLineJoin lineJoin, nfloat miterLimit)
	{
		return MakeMutable(CGPathCreateCopyByStrokingPath(handle, null, lineWidth, lineCap, lineJoin, miterLimit));
	}

	[DllImport("/System/Library/Frameworks/ApplicationServices.framework/Versions/A/Frameworks/CoreGraphics.framework/CoreGraphics")]
	private static extern IntPtr CGPathCreateCopyByTransformingPath(IntPtr path, ref CGAffineTransform transform);

	public CGPath CopyByTransformingPath(CGAffineTransform transform)
	{
		return MakeMutable(CGPathCreateCopyByTransformingPath(handle, ref transform));
	}

	[DllImport("/System/Library/Frameworks/ApplicationServices.framework/Versions/A/Frameworks/CoreGraphics.framework/CoreGraphics")]
	private static extern IntPtr CGPathCreateMutableCopyByTransformingPath(IntPtr path, ref CGAffineTransform transform);

	[DllImport("/System/Library/Frameworks/ApplicationServices.framework/Versions/A/Frameworks/CoreGraphics.framework/CoreGraphics")]
	private unsafe static extern IntPtr CGPathCreateWithEllipseInRect(CGRect boundingRect, CGAffineTransform* transform);

	public unsafe static CGPath EllipseFromRect(CGRect boundingRect, CGAffineTransform transform)
	{
		return MakeMutable(CGPathCreateWithEllipseInRect(boundingRect, &transform));
	}

	public unsafe static CGPath EllipseFromRect(CGRect boundingRect)
	{
		return MakeMutable(CGPathCreateWithEllipseInRect(boundingRect, null));
	}

	[DllImport("/System/Library/Frameworks/ApplicationServices.framework/Versions/A/Frameworks/CoreGraphics.framework/CoreGraphics")]
	private unsafe static extern IntPtr CGPathCreateWithRect(CGRect boundingRect, CGAffineTransform* transform);

	public unsafe static CGPath FromRect(CGRect rectangle, CGAffineTransform transform)
	{
		return MakeMutable(CGPathCreateWithRect(rectangle, &transform));
	}

	public unsafe static CGPath FromRect(CGRect rectangle)
	{
		return MakeMutable(CGPathCreateWithRect(rectangle, null));
	}

	[DllImport("/System/Library/Frameworks/ApplicationServices.framework/Versions/A/Frameworks/CoreGraphics.framework/CoreGraphics")]
	private unsafe static extern IntPtr CGPathCreateWithRoundedRect(CGRect rect, nfloat cornerWidth, nfloat cornerHeight, CGAffineTransform* transform);

	private unsafe static CGPath _FromRoundedRect(CGRect rectangle, nfloat cornerWidth, nfloat cornerHeight, CGAffineTransform* transform)
	{
		if (cornerWidth < 0 || 2 * cornerWidth > rectangle.Width)
		{
			throw new ArgumentException("cornerWidth");
		}
		if (cornerHeight < 0 || 2 * cornerHeight > rectangle.Height)
		{
			throw new ArgumentException("cornerHeight");
		}
		return MakeMutable(CGPathCreateWithRoundedRect(rectangle, cornerWidth, cornerHeight, transform));
	}

	[Mac(10, 9)]
	[iOS(7, 0)]
	public unsafe static CGPath FromRoundedRect(CGRect rectangle, nfloat cornerWidth, nfloat cornerHeight)
	{
		return _FromRoundedRect(rectangle, cornerWidth, cornerHeight, null);
	}

	[Mac(10, 9)]
	[iOS(7, 0)]
	public unsafe static CGPath FromRoundedRect(CGRect rectangle, nfloat cornerWidth, nfloat cornerHeight, CGAffineTransform transform)
	{
		return _FromRoundedRect(rectangle, cornerWidth, cornerHeight, &transform);
	}

	[DllImport("/System/Library/Frameworks/ApplicationServices.framework/Versions/A/Frameworks/CoreGraphics.framework/CoreGraphics")]
	private unsafe static extern void CGPathAddRoundedRect(IntPtr path, CGAffineTransform* transform, CGRect rect, nfloat cornerWidth, nfloat cornerHeight);

	[Mac(10, 9)]
	[iOS(7, 0)]
	public unsafe void AddRoundedRect(CGAffineTransform transform, CGRect rect, nfloat cornerWidth, nfloat cornerHeight)
	{
		CGPathAddRoundedRect(handle, &transform, rect, cornerWidth, cornerHeight);
	}

	[Mac(10, 9)]
	[iOS(7, 0)]
	public unsafe void AddRoundedRect(CGRect rect, nfloat cornerWidth, nfloat cornerHeight)
	{
		CGPathAddRoundedRect(handle, null, rect, cornerWidth, cornerHeight);
	}
}
