using ESRI.ArcGIS.esriSystem;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ESRI.ArcGIS.Carto;
using ESRI.ArcGIS.Display;
using ESRI.ArcGIS.Geodatabase;
using ESRI.ArcGIS.Geometry;

namespace _5_8AOI
{
    class MapComposer
    {
        //获取渲染器类型
        public static string GetRendererTypeByLayer(ILayer layer)
        {
            //判断图层是否获取成功
            if (layer == null)
            {
                return "图层获取失败";
            }
            //使用IGeoFeatueLayer接口访问指定图层，并获取其渲染器
            IFeatureLayer featureLayer = layer as IFeatureLayer;
            IGeoFeatureLayer geoFeatureLayer = layer as IGeoFeatureLayer;
            IFeatureRenderer featureRenderer = geoFeatureLayer.Renderer;
            //判断该图层渲染器是否为备选渲染器类型之一
            if (featureRenderer is ISimpleRenderer)
            {
                return "SimpleRender";
            }
            else if (featureRenderer is IUniqueValueRenderer)
            {
                return "UniqueValueRenderer";
            }
            else if (featureRenderer is IDotDensityRenderer)
            {
                return "DotDensityRenderer";
            }
            else if (featureRenderer is IChartRenderer)
            {
                return "ChartRenderer";
            }
            else if (featureRenderer is IProportionalSymbolRenderer)
            {
                return "ProportionalSymbolRenderer";
            }
            else if (featureRenderer is IRepresentationRenderer)
            {
                return "RepresentationRenderer";
            }
            else if (featureRenderer is IClassBreaksRenderer)
            {
                return "ClassBreaksRenderer";
            }
            else if (featureRenderer is IBivariateRenderer)
            {
                return "BivariateRenderer";
            }
            //若渲染器匹配失败
            return "未知或渲染器匹配失败";
        }
        //添加函数，用于获取指定图层的符号信息
        public static ISymbol GetSymbolFromLayer(ILayer layer)
        {
            //判断图层是否获取成功
            if (layer == null)
            {
                return null;
            }
            //利用IFeatureLayer接口访问指定图层，获取到图层中的第一个要素
            IFeatureLayer featureLayer = layer as IFeatureLayer;
            IFeatureCursor featureCursor = featureLayer.Search(null, false);
            IFeature feature = featureCursor.NextFeature();
            if (feature == null)
            {
                return null;
            }
            //利用IGeoFeatureLayer访问指定图层，获取其渲染器
            IGeoFeatureLayer geoFeatureLayer = featureLayer as IGeoFeatureLayer;
            IFeatureRenderer featureRenderer = geoFeatureLayer.Renderer;
            if (featureRenderer == null)
            {
                return null;
            }
            //通过IFeatureRenderer接口的方法获取图层要素对应的符号信息，并作为函数结果返回
            ISymbol symbol = featureRenderer.get_SymbolByFeature(feature);
            return symbol;
        }
        //添加静态成员函数RenderSimply,用于统一设置指定图层符号的颜色，并进行简单渲染
        public static bool RenderSimply(ILayer layer, IColor color)
        {
             //判断图层和颜色是否获取成功
            if (layer==null||color==null)
	        {
                return false;
	        }
            //调用GetSymbolFromLayer成员函数，获取指定图层的符号
            ISymbol symbol = GetSymbolFromLayer(layer);
            if (symbol == null)
            {
                return false;
            }
            //获取指定图层的要素类
            IFeatureLayer featureLayer = layer as IFeatureLayer;
            IFeatureClass featureClass = featureLayer.FeatureClass;
            if (featureClass == null)
            {
                return false;
            }
             //获取指定图层要素类的几何形状信息，并进行匹配。根据不同形状，设置不同类型符号的颜色
            esriGeometryType geotype = featureClass.ShapeType;      //枚举类型：定义的一组常数值
            switch (geotype)
            {
                case esriGeometryType.esriGeometryPoint:
                    {
                        IMarkerSymbol markerSymbol = symbol as IMarkerSymbol;
                        markerSymbol.Color = color;
                        break;
                    }
                case esriGeometryType.esriGeometryMultipoint:
                    {
                        IMarkerSymbol markerSymbol = symbol as IMarkerSymbol;
                        markerSymbol.Color = color;
                        break;
                    }
                case esriGeometryType.esriGeometryPolyline:
                    {
                        ISimpleLineSymbol simpleLineSymbol = symbol as ISimpleLineSymbol;
                        simpleLineSymbol.Color = color;
                        break;
                    }
                case esriGeometryType.esriGeometryPolygon:
                    {
                        IFillSymbol fillSymbol = symbol as IFillSymbol;
                        break;
                    }
                default:
                    return false;
            }
            //新建简单渲染器对象，设置其符号
            ISimpleRenderer simpleRender = new SimpleRendererClass();
            simpleRender.Symbol = symbol;
            IFeatureRenderer featureRenderer = simpleRender as IFeatureRenderer;
            if (featureRenderer == null)
            {
                return false;
            }
            //通过IGeoFeatureLayer接口访问指定图层，并设置其渲染器。
            IGeoFeatureLayer geoFeatureLayer = featureLayer as IGeoFeatureLayer;
            geoFeatureLayer.Renderer = featureRenderer;
            return true;
        }

    }
}

