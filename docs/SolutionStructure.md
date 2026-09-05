```mermaid
flowchart BT
	HallRentingService.Data ==> HallRentingService.WebAPI
	HallRentingService.ServiceDefaults ==> HallRentingService.WebAPI
	HallRentingService.WebAPI ==> HallRentingService.AppHost
	HallRentingService.WebAPI ==> HallRentingService.UnitTests
	HallRentingService.WebAPI ==> HallRentingService.IntegrationTests
	HallRentingService.AppHost ==> HallRentingService.IntegrationTests
```