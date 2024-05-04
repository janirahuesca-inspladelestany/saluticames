﻿using SharedKernel.Common;

namespace Domain.CatalogueContext.Errors;

public static class CatalogueErrors
{
    public static readonly Error CatalogueIdNotFound = Error.NotFound(
        "CatalogueErrors.CatalogueIdNotFound", "CatalogueId not found.");

    public static readonly Error RegionNotAvailable = Error.Conflict(
        "CatalogueErrors.RegionNotAvailable", "Region is not available.");

    public static readonly Error InvalidAltitude = Error.Validation(
            "CatalogueErrors.InvalidAltitude", "Altitude is not valid.");
}