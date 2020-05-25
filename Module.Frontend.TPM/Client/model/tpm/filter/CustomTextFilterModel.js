Ext.define('App.model.tpm.filter.CustomTextFilterModel', {
    extend: 'App.util.core.ObservableMap',
    
    filterOperationMap: {
        'Equals': breeze.FilterQueryOp.Equals.operator,
        'NotEqual': breeze.FilterQueryOp.NotEquals.operator,
        'GreaterThan': breeze.FilterQueryOp.GreaterThan.operator,
        'GreaterOrEqual': breeze.FilterQueryOp.GreaterThanOrEqual.operator,
        'LessThan': breeze.FilterQueryOp.LessThan.operator,
        'LessOrEqual': breeze.FilterQueryOp.LessThanOrEqual.operator,
        'IsNull': 'isnull',//breeze.FilterQueryOp.Equals.operator,
        'NotNull': 'notnull',//breeze.FilterQueryOp.NotEquals.operator,
        'Contains': breeze.FilterQueryOp.Contains.operator,
        'NotContains': 'notcontains',
        'StartsWith': breeze.FilterQueryOp.StartsWith.operator,
        'EndsWith': breeze.FilterQueryOp.EndsWith.operator,
        'In': 'in'
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
                //нельзя оставлять значение undefined, потому что с таким значением получится некорректная json строка
                op[opName] = node.value ? node.value : '';
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
})