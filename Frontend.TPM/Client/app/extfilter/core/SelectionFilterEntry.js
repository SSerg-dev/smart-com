Ext.define('App.extfilter.core.SelectionFilterEntry', {
    extend: 'App.util.core.ObservableMap',

    typeOperationsMap: {
        'string': ['Equals', 'NotEqual', 'In', 'IsNull', 'NotNull', 'Contains', 'NotContains', 'StartsWith', 'EndsWith'],
        'int': ['Equals', 'NotEqual', 'GraterThan', 'GraterOrEqual', 'LessThan', 'LessOrEqual', 'Between', 'In', 'IsNull', 'NotNull'],
        'float': ['Equals', 'NotEqual', 'GraterThan', 'GraterOrEqual', 'LessThan', 'LessOrEqual', 'Between', 'In', 'IsNull', 'NotNull'],
        'boolean': ['Equals', 'NotEqual', 'IsNull', 'NotNull'],
        'date': ['Equals', 'NotEqual', 'GraterThan', 'GraterOrEqual', 'LessThan', 'LessOrEqual', 'Between', 'In', 'IsNull', 'NotNull']
    },
    configAllowedOperations: undefined,
    operationValueTypesMap: {
        'Equals': ['atom', 'field'],
        'NotEqual': ['atom', 'field'],
        'GraterThan': ['atom', 'field'],
        'GraterOrEqual': ['atom', 'field'],
        'LessThan': ['atom', 'field'],
        'LessOrEqual': ['atom', 'field'],
        'Contains': ['atom', 'field'],
        'NotContains': ['atom', 'field'],
        'StartsWith': ['atom', 'field'],
        'EndsWith': ['atom', 'field'],
        'In': ['list', 'searchlist'],
        'Between': ['range']
    },

    typeDefaultOperationMap: {
        'string': 'Contains',
        'int': 'Equals',
        'float': 'Equals',
        'boolean': 'Equals',
        'date': 'Between'
    },

    defaultData: {
        id: null,
        name: '',
        operation: null,
        field: '',
        fieldType: '',
        value: null,
        metadata: null
    },

    constructor: function (model, data, config) {
        this.applyConfig(config);
        this.configAllowedOperations = config.allowedOperations;
        data = data || this.defaultData;
        var t = this.getCorrectedFieldType(data.fieldType);
        data.operation = data.operation || this.defaultOperations[t];

        this.callParent([data]);
        this.model = model;
    },
   
    applyConfig: function (config) {
        var configProto = {
            allowedOperations: Ext.apply({}, this.typeOperationsMap),
            valueTypes: Ext.apply({}, this.operationValueTypesMap),
            defaultOperations: Ext.apply({}, this.typeDefaultOperationMap),
            editors: {},
            values: {}
        };

        var overrides = App.Map.pull(config || {}, Ext.Object.getKeys(configProto), null, true);

        Ext.apply(this, Ext.Object.merge(configProto, overrides));
    },

    getModel: function () {
        return this.model;
    },

    getCorrectedFieldType: function (fieldType) {
        if (fieldType == 'bool') {
            return 'boolean';
        } else {
            return fieldType;
        }
    },

    getAllowedOperations: function () {
        var t = this.getCorrectedFieldType(this.get('fieldType'));
        return this.configAllowedOperations && this.configAllowedOperations[t] ? this.configAllowedOperations[t] : this.allowedOperations[t];
    },

    getAllowedValueTypes: function () {
        return this.valueTypes[this.get('operation')] || [];
    },

    getAllowedModelFields: function () {
        return this.getModel().getFields().filter(function (field) {
            var t1 = this.getCorrectedFieldType(this.get('fieldType'));
            var t2 = this.getCorrectedFieldType(field.type.type);
            return ((Ext.isFunction(field.hidden) && !field.hidden()) || !field.hidden) && t1 === t2;
        }, this);
    },

    getDefaultValueType: function () {
        return this.getAllowedValueTypes()[0] || null;
    },

    isAllowedRelative: function () {
        return Ext.Array.contains(this.getAllowedValueTypes(), 'field');
    },

    isAllowedRange: function () {
        return Ext.Array.contains(this.getAllowedOperations(), 'Between');
    },

    getValueType: function () {
        var value = this.get('value');

        if (value === null || value === undefined) {
            return this.getDefaultValueType();
        }

        switch (Ext.getClassName(value)) {
            case 'App.extfilter.core.Field':
                return 'field';
            case 'App.extfilter.core.Range':
                return 'range';
            case 'App.extfilter.core.ValueList':
            case 'App.extfilter.core.RangeList':
                return 'list';
            case 'App.extfilter.core.ValueSearchList':
                return this.getDefaultValueType();
            default:
                return 'atom';
        }
    }

});