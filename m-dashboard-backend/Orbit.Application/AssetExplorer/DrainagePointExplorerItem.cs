using Orbit.Models.Assets;
using CoordinateConversionService.CRS;
using CoordinateConversionService.Services;
using CoordinateConversionService.Utility;
using System.Linq;
using System;

namespace Orbit.Application.AssetExplorer
{
    public class DrainagePointExplorerItem:AssetExplorerItem
    {
        public DrainagePointExplorerItem(DrainagePoint dp)
        {
            Id = dp.Id.ToString();
            DisplayName = dp.Name;
            WellName = dp.Well.Name;
            WellId = dp.Well.Id;
            if (dp.Well.WellDeviation != null && dp.Well.SurfaceCoordinateX > 0 && dp.Well.SurfaceCoordinateY > 0)
                SetGeodeticCoordinate(dp.Well.WellDeviation.AreaRegion, dp.Well.WellDeviation.GeodeticDatum,
                                  dp.Well.WellDeviation.CoordinateReferenceSystem,
                                  dp.Well.SurfaceCoordinateX, dp.Well.SurfaceCoordinateY);
        }

       

        private void SetGeodeticCoordinate(int areaCode, int datumCode, int crsCode, double x, double y)
        {
            try
            {
                if (areaCode > 0 && datumCode > 0 && crsCode > 0)
                {
                    if (_geodeticService == null)
                        _geodeticService = new GeodeticService();

                    GridCoord coord = new GridCoord(x * 0.3048, y * 0.3048);

                    var datum = _geodeticService.GetDatums(areaCode)
                                                .FirstOrDefault(dat => dat.Code == datumCode);
                    var crs = _geodeticService.GetCoordinateReferenceSystems(areaCode, datumCode)
                                              .FirstOrDefault(cor => cor.Code == crsCode);

                    var map = _geodeticService.GetMapProjectionInfo(crs);
                    var result = _geodeticService.ConvertFromUtmToGcs(coord, datum.Ellipsoid, map);

                    Latitude = CrsConverter.ConvertRadianToDegree(result.Latitude);
                    Longitude = CrsConverter.ConvertRadianToDegree(result.Longitude);
                }
            }
            catch { }
        }

        public double? Longitude { get; private set; }
        public double? Latitude { get; private set; }
        public string WellName { get; private set; }
        public Guid WellId { get; private set; }

        private IGeodeticService _geodeticService;
    }
}
