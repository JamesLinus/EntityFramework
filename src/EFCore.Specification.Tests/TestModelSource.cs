// Copyright (c) .NET Foundation. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

using System;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Internal;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Metadata.Conventions.Internal;
using Microsoft.EntityFrameworkCore.Metadata.Internal;
using Microsoft.Extensions.DependencyInjection;

namespace Microsoft.EntityFrameworkCore
{
    public class TestModelSource : ModelSource
    {
        private readonly Action<ModelBuilder> _onModelCreating;

        public TestModelSource(Action<ModelBuilder> onModelCreating, ModelSourceDependencies dependencies)
            : base(dependencies)
        {
            _onModelCreating = onModelCreating;
        }

        protected override IModel CreateModel(DbContext context, IConventionSetBuilder conventionSetBuilder, IModelValidator validator)
        {
            var conventionSet = CreateConventionSet(conventionSetBuilder);

            var modelBuilder = new ModelBuilder(conventionSet);
            var model = modelBuilder.GetInfrastructure().Metadata;
            model.SetProductVersion(ProductInfo.GetVersion());

            Dependencies.ModelCustomizer.Customize(modelBuilder, context);

            _onModelCreating(modelBuilder);

            model.Validate();

            validator.Validate(model);

            return model;
        }

        public static Func<IServiceProvider, IModelSource> GetFactory(Action<ModelBuilder> onModelCreating)
            => p => new TestModelSource(
                onModelCreating,
                p.GetRequiredService<ModelSourceDependencies>());
    }
}
