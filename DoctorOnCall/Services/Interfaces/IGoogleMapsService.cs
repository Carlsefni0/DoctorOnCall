using Azure.Core.GeoJson;
using DoctorOnCall.Controllers;
using DoctorOnCall.DTOs.Route;
using NetTopologySuite.Geometries;

namespace DoctorOnCall.Services.Interfaces;

public interface IGoogleMapsService
{
    Task<Point> GetCoordinates(string address);
    Task<RouteInfoDto> GetRouteInfo(Point origin, Point destination,string mode);
}