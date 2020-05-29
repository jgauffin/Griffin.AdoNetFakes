using System;
using System.Reflection;
using Griffin.AdoNetFakes.Tests.SimpleData;
using Shouldly;
using Xunit;

namespace Griffin.AdoNetFakes.Tests
{
    public class DataTableExtensionsTests
    {
        [Fact]
        public void ToTable_can_map_a_SimpleObject_instance()
        {
            // Arrange
            var instances = new[]
            {
                new SimpleObject() { Id = 1, Name = "Bob", DateOfBirth = DateTime.Parse("1995-06-23")}
            };

            // Act
            var result = instances.ToDataTable();

            // Assert
            result.ShouldNotBeNull();

            result.Columns.Count.ShouldBe(3);
            result.Columns[0].ColumnName.ShouldBe(nameof(SimpleObject.Id));
            result.Columns[1].ColumnName.ShouldBe(nameof(SimpleObject.Name));
            result.Columns[2].ColumnName.ShouldBe(nameof(SimpleObject.DateOfBirth));

            result.Rows.Count.ShouldBe(instances.Length);

            for (var i = 0; i < instances.Length; ++i)
            {
                result.Rows[i][nameof(SimpleObject.Id)].ShouldBe(instances[i].Id);
                result.Rows[i][nameof(SimpleObject.Name)].ShouldBe(instances[i].Name);
                result.Rows[i][nameof(SimpleObject.DateOfBirth)].ShouldBe(instances[i].DateOfBirth);
            }
        }

        [Fact]
        public void ToTable_can_map_multiple_SimpleObject_instances()
        {
            // Arrange
            var instances = new[]
            {
                new SimpleObject() { Id = 1, Name = "Bob", DateOfBirth = DateTime.Parse("1995-06-23")},
                new SimpleObject() { Id = 2, Name = "Dave", DateOfBirth = DateTime.Parse("1996-10-10")},
                new SimpleObject() { Id = 3, Name = "Jim", DateOfBirth = DateTime.Parse("1994-02-18")},
                new SimpleObject() { Id = 4, Name = "Rich", DateOfBirth = DateTime.Parse("1995-03-30")}
            };

            // Act
            var result = instances.ToDataTable();

            // Assert
            result.ShouldNotBeNull();

            result.Columns.Count.ShouldBe(3);
            result.Columns[0].ColumnName.ShouldBe(nameof(SimpleObject.Id));
            result.Columns[1].ColumnName.ShouldBe(nameof(SimpleObject.Name));
            result.Columns[2].ColumnName.ShouldBe(nameof(SimpleObject.DateOfBirth));

            result.Rows.Count.ShouldBe(instances.Length);
            for (var i = 0; i < instances.Length; ++i)
            {
                result.Rows[i][nameof(SimpleObject.Id)].ShouldBe(instances[i].Id);
                result.Rows[i][nameof(SimpleObject.Name)].ShouldBe(instances[i].Name);
                result.Rows[i][nameof(SimpleObject.DateOfBirth)].ShouldBe(instances[i].DateOfBirth);
            }
        }

        [Fact]
        public void ToTable_can_map_a_SimpleObject_instance_and_only_include_specific_columns()
        {
            // Arrange
            var instances = new[]
            {
                new SimpleObject() { Id = 1, Name = "Bob", DateOfBirth = DateTime.Parse("1995-06-23")}
            };

            // Act
            var result = instances.ToDataTable(includedPropertyNames: new[] { nameof(SimpleObject.Id), nameof(SimpleObject.Name) });

            // Assert
            result.ShouldNotBeNull();

            result.Columns.Count.ShouldBe(2);
            result.Columns[0].ColumnName.ShouldBe(nameof(SimpleObject.Id));
            result.Columns[1].ColumnName.ShouldBe(nameof(SimpleObject.Name));

            result.Rows.Count.ShouldBe(instances.Length);

            for (var i = 0; i < instances.Length; ++i)
            {
                result.Rows[i][nameof(SimpleObject.Id)].ShouldBe(instances[i].Id);
                result.Rows[i][nameof(SimpleObject.Name)].ShouldBe(instances[i].Name);
            }
        }

        [Fact]
        public void ToTable_can_map_a_SimpleObject_instance_with_binding_flags_for_specific_properties()
        {
            // Arrange
            var instances = new[]
            {
                new SimpleObject() { Id = 1, Name = "Bob", DateOfBirth = DateTime.Parse("1995-06-23")}
            };

            var bindingFlags = BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic;

            // Act
            var result = instances.ToDataTable(bindingFlags);

            // Assert
            result.ShouldNotBeNull();

            result.Columns.Count.ShouldBe(4);
            result.Columns[0].ColumnName.ShouldBe(nameof(SimpleObject.Id));
            result.Columns[1].ColumnName.ShouldBe(nameof(SimpleObject.Name));
            result.Columns[2].ColumnName.ShouldBe(nameof(SimpleObject.DateOfBirth));
            result.Columns[3].ColumnName.ShouldBe(nameof(SimpleObject.ApproxUnixAge));

            result.Rows.Count.ShouldBe(instances.Length);

            for (var i = 0; i < instances.Length; ++i)
            {
                result.Rows[i][nameof(SimpleObject.Id)].ShouldBe(instances[i].Id);
                result.Rows[i][nameof(SimpleObject.Name)].ShouldBe(instances[i].Name);
                result.Rows[i][nameof(SimpleObject.DateOfBirth)].ShouldBe(instances[i].DateOfBirth);
                result.Rows[i][nameof(SimpleObject.ApproxUnixAge)].ShouldBe(instances[i].ApproxUnixAge);
            }
        }
    }
}
