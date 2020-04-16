Ext.define('App.extfilter.core.TextFilterModel', {
    extend: 'App.util.core.ObservableMap',
    alternateClassName: 'App.ExtTextFilterModel',

    canUpdateFromFilter: true,

    filterOperationMap: {
        'Equals': breeze.FilterQueryOp.Equals.operator,
        'NotEqual': breeze.FilterQueryOp.NotEquals.operator,
        'GreaterThen': breeze.FilterQueryOp.GreaterThan.operator,
        'GreaterOrEqual': breeze.FilterQueryOp.GreaterThanOrEqual.operator,
        'LessThan': breeze.FilterQueryOp.LessThan.operator,
        'LessOrEqual': breeze.FilterQueryOp.LessThanOrEqual.operator,
        'IsNull': breeze.FilterQueryOp.Equals.operator,
        'NotNull': breeze.FilterQueryOp.NotEquals.operator,
        'Contains': breeze.FilterQueryOp.Contains.operator,
        'NotContains': 'notcontains',
        'StartsWith': breeze.FilterQueryOp.StartsWith.operator,
        'EndsWith': breeze.FilterQueryOp.EndsWith.operator
    },

    config: {
        modelId: null
    },

    defaultData: {
        filterText: ''
    },

    constructor: function (config) {
        this.callParent();
        this.initConfig(config);
    },

    getFilter: function () {
        var filterText = this.get('filterText');

        if (!Ext.isEmpty(filterText)) {
            try {
                var filter = Ext.JSON.decode(filterText);
                return this.deserializeFilter(filter);
            } catch (e) {
                console.error('Parse extended text filter error: ', e);
            }
        }

        return null;
    },

    updateFromFilter: function (filter) {
        var filterText = JSON.stringify(this.serializeFilter(filter), null, 4);
        this.set('filterText', filterText);
        this.commit();
    },

    getView: function () {
        return Ext.widget('exttextfilter', this);
    },

    serializeFilter: function (node) {
        if (Ext.isArray(node)) {
            return Ext.Array.map(node, function (item) {
                return this.serializeFilter(item);
            }, this);
        } else if (Ext.isObject(node)) {
            if (node.operator) {
                var obj = {};
                obj[node.operator] = this.serializeFilter(node.rules);
                return obj;
            } else {
                var opName = this.filterOperationMap[node.operation],
                    prop = {},
                    op = {};
                op[opName] = node.value;
                prop[node.property] = op;
                return prop;
            }
        } else {
            //TODO: ?
        }
    },

    deserializeFilter: function (node, result) {
        if (Ext.isArray(node)) {
            return Ext.Array.map(node, function (item) {
                return this.deserializeFilter(item);
            }, this);
        } else if (Ext.isObject(node)) {
            var obj = result || {};
            Ext.Object.each(node, function (key, value) {
                var op = this.getOperation(key.toLowerCase());
                if (key === 'and' || key === 'or') {
                    obj.operator = key;
                    obj.rules = this.deserializeFilter(value);
                } else if (op) {
                    obj.operation = op;
                    obj.value = value;
                } else {
                    obj.property = key;
                    this.deserializeFilter(value, obj);
                }
            }, this);
            return obj;
        } else {
            //TODO: ?
        }
    },

    getOperation: function (op) {
        return Ext.Object.getKey(this.filterOperationMap, op);
    }

});