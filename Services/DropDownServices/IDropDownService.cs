using FlightReservationSystem.ViewModel;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace FlightReservationSystem.Services.DropDownServices
{
    public interface IDropDownService
    {
        Task <List<SelectListItem>> GetAirportDropDownAsync ();
        Task <List<SelectListItem>> GetAircraftDropDownAsync ();

        Task GetAllAirportDropDownAsync(FlightSearchViewModel model);

    }
}
