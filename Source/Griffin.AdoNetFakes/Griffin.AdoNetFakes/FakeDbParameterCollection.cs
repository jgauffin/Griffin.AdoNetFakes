using System;
using System.Collections;
using System.Collections.Generic;
using System.Data.Common;
using System.Linq;

namespace Griffin.AdoNetFakes;

public class FakeDbParameterCollection : DbParameterCollection, IEnumerable<FakeParameter>
{
    private readonly List<FakeParameter> _fakeParameters = new();

    /// <summary>
    ///     Initializes a new instance of the <see cref="FakeDbParameterCollection" /> class.
    /// </summary>
    /// <param name="command">The commandResult.</param>
    public FakeDbParameterCollection(FakeCommand command)
    {
        Command = command;
        if (command.FakeParameters != null)
        {
            _fakeParameters.AddRange(command.FakeParameters);
        }
    }

    /// <summary>
    ///     Initializes a new instance of the <see cref="FakeDbParameterCollection" /> class.
    /// </summary>
    /// <param name="parameters">The parameters.</param>
    public FakeDbParameterCollection(FakeDbParameterCollection parameters)
    {
        Command = parameters.Command;
        _fakeParameters.AddRange(parameters);
    }

    public FakeDbParameterCollection(FakeParameter parameter)
    {
        _fakeParameters.Add(parameter);
    }

    public FakeDbParameterCollection()
    {
    }

    public FakeCommand Command { get; set; }

    public override int Count => _fakeParameters.Count;

    public override object SyncRoot => _fakeParameters;

    IEnumerator<FakeParameter> IEnumerable<FakeParameter>.GetEnumerator()
    {
        return _fakeParameters.GetEnumerator();
    }

    public override IEnumerator GetEnumerator()
    {
        return _fakeParameters.GetEnumerator();
    }

    /// <summary>
    ///     Adds the specified <see cref="T:System.Data.Common.DbParameter" /> object to the
    ///     <see cref="T:System.Data.Common.DbParameterCollection" />.
    /// </summary>
    /// <param name="value">
    ///     The <see cref="P:System.Data.Common.DbParameter.Value" /> of the
    ///     <see cref="T:System.Data.Common.DbParameter" /> to add to the collection.
    /// </param>
    /// <returns>The index of the <see cref="T:System.Data.Common.DbParameter" /> object in the collection.</returns>
    public override int Add(object value)
    {
        var index = _fakeParameters.Count;
        _fakeParameters.Add((FakeParameter)value);
        return index;
    }

    public override bool Contains(object value)
    {
        return _fakeParameters.Contains(value);
    }

    public override void Clear()
    {
        _fakeParameters.Clear();
    }

    public override int IndexOf(object value)
    {
        return _fakeParameters.IndexOf((FakeParameter)value);
    }

    public override void Insert(int index, object value)
    {
        _fakeParameters.Insert(index, (FakeParameter)value);
    }

    public override void Remove(object value)
    {
        _fakeParameters.Remove((FakeParameter)value);
    }

    public override void RemoveAt(int index)
    {
        _fakeParameters.RemoveAt(index);
    }

    public override void RemoveAt(string parameterName)
    {
        _fakeParameters.RemoveAll(x => x.ParameterName == parameterName);
    }

    protected override void SetParameter(int index, DbParameter value)
    {
        _fakeParameters[index] = (FakeParameter)value;
    }

    protected override void SetParameter(string parameterName, DbParameter value)
    {
        var current = _fakeParameters.FirstOrDefault(x => x.ParameterName == parameterName);
        if (current != null)
        {
            current.Value = value;
        }

        _fakeParameters.Add(new FakeParameter(parameterName, value.Value));
    }

    public override int IndexOf(string parameterName)
    {
        return _fakeParameters.FindIndex(x => x.ParameterName == parameterName);
    }

    protected override DbParameter GetParameter(int index)
    {
        return _fakeParameters[index];
    }

    protected override DbParameter GetParameter(string parameterName)
    {
        return _fakeParameters.FirstOrDefault(x => x.ParameterName == parameterName);
    }

    public override bool Contains(string value)
    {
        return _fakeParameters.Any(x => x.ParameterName == value);
    }

    public override void CopyTo(Array array, int index)
    {
        for (var i = 0; i < _fakeParameters.Count; i++)
        {
            array.SetValue(_fakeParameters[i], index + i);
        }
    }

    public override void AddRange(Array values)
    {
        foreach (var value in values)
        {
            Add(value);
        }
    }
}
