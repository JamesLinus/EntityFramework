// Copyright (c) .NET Foundation. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

using Microsoft.EntityFrameworkCore.Diagnostics;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.TestModels.Northwind;

namespace Microsoft.EntityFrameworkCore.Query
{
    public abstract class NorthwindQueryFixtureBase<TModelCustomizer> : SharedStoreFixtureBase<NorthwindContext>
        where TModelCustomizer : IModelCustomizer, new()
    {
        protected override string StoreName { get; } = "Northwind";

        protected override void OnModelCreating(ModelBuilder modelBuilder, DbContext context)
            => new TModelCustomizer().Customize(modelBuilder, context);

        protected override void Seed(NorthwindContext context) => NorthwindData.Seed(context);

        protected override DbContextOptionsBuilder AddOptions(DbContextOptionsBuilder builder)
            => base.AddOptions(builder).ConfigureWarnings(c => c
                .Log(CoreEventId.RowLimitingOperationWithoutOrderByWarning)
                .Log(CoreEventId.FirstWithoutOrderByAndFilterWarning)
                .Log(CoreEventId.PossibleUnintendedCollectionNavigationNullComparisonWarning)
                .Log(CoreEventId.PossibleUnintendedReferenceComparisonWarning));
    }
}
