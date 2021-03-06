// Copyright (c) .NET Foundation. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

using Microsoft.EntityFrameworkCore.Utilities;

namespace Microsoft.EntityFrameworkCore
{
    public class F1SqliteFixture : F1RelationalFixture<SqliteTestStore>
    {
        protected override TestStoreFactory<SqliteTestStore> TestStoreFactory => SqliteTestStoreFactory.Instance;
    }
}
