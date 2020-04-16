Ext.define('App.extfilter.core.SelectionFilterModel', {
    extend: 'App.util.core.ObservableArray',
    alternateClassName: 'App.ExtSelectionFilterModel',

    canUpdateFromFilter: false,

    config: {
        modelId: null,
        model: null
    },

    constructor: function (config) {
        this.callParent(arguments);
        this.initConfig(config);
        this.init();
    },

    init: function () {
        this.entries.addAll(this.createFilterEntries(this.getFields(true)));
    },

    applyModel: function (model) {
        return Ext.ModelManager.getModel(model);
    },

    getFilter: function () {
        var nodes = this.getFilterEntries().map(function (item) {
            var operation = item.get('operation'),
                field = item.get('field'),
                fieldType = item.get('fieldType'),
                value = item.get('value'),
                metadata = item.get('metadata');

            switch (operation) {
                case 'Between':
                    return this.parseRange(field, value, metadata);
                case 'In':
                    return this.parseList(field, value, metadata);
                case 'IsNull':
                case 'NotNull':
                    if (fieldType === 'string') {
                        var nullRule = this.makeRule(field, operation, null, true),
                            emptyRule = this.makeRule(field, operation === 'IsNull' ? 'Equals' : 'NotEqual', '', true);

                        return this.makeNode(operation === 'IsNull' ? 'or' : 'and', [nullRule, emptyRule]);
                    } else {
                        return this.makeRule(field, operation, null, true);
                    }
            }

            return this.parseField(field, operation, value, metadata);
        }, this).filter(function (node) {
            return !Ext.isEmpty(node);
        });

        return this.makeNode('and', nodes);
    },

    parseRange: function (property, value, metadata) {
        if (value && Ext.getClassName(value) === 'App.extfilter.core.Range') {
            var from = value.from,
                to = value.to,
                property1, property2;

            property1 = property2 = property;

            if (from instanceof App.MarsDate && to instanceof App.MarsDate) {
                from = from.getStartDate();
                to = to.getEndDate();
                property1 = metadata && metadata.MDStartFieldName || 'StartDate';
                property2 = metadata && metadata.MDEndFieldName || 'FinishDate';
            }

            var nodes = [
                this.makeRule(property1, 'GreaterOrEqual', from),
                this.makeRule(property2, 'LessOrEqual', to)
            ].filter(function (node) {
                return !Ext.isEmpty(node);
            });

            return this.makeNode('and', nodes);
        }

        return null;
    },

    parseRangeList: function (property, value) {
        if (value && Ext.getClassName(value) === 'App.extfilter.core.RangeList') {
            return Ext.Array.from(value.ranges).map(function (item) {
                return this.parseRange(property, item);
            }, this).filter(function (node) {
                return !Ext.isEmpty(node);
            });
        }

        return null;
    },

    parseValueList: function (property, value) {
        if (value && Ext.getClassName(value) === 'App.extfilter.core.ValueList') {
            return Ext.Array.from(value.values).map(function (item) {
                return this.makeRule(property, 'Equals', item);
            }, this).filter(function (node) {
                return !Ext.isEmpty(node);
            });
        }

        return null;
    },

    parseValueSearchList: function (property, value) {
        if (value && Ext.getClassName(value) === 'App.extfilter.core.ValueSearchList') {
            return Ext.Array.clean(value.values.map(function (item) {
                return this.makeRule(property, 'Equals', item.value);
            }, this));
        }

        return null;
    },

    parseList: function (property, value) {
        var nodes = this.parseRangeList(property, value) || this.parseValueList(property, value) || this.parseValueSearchList(property, value);
        return this.makeNode('or', nodes);
    },

    parseField: function (property, operation, value, metadata) {
        if (value && Ext.getClassName(value) === 'App.extfilter.core.Field') {
            return this.makeRule(property, operation, value.name);
        }

        return this.makeRule(property, operation, value, false, metadata);
    },

    makeRule: function (property, operation, value, allowEmpty, metadata) {
        if (!Ext.isEmpty(property) && !Ext.isEmpty(operation) && (allowEmpty || !Ext.isEmpty(value))) {

            if (value instanceof App.MarsDate) {
                var nodes = [
                    this.makeRule(metadata && metadata.MDStartFieldName || 'StartDate', 'GreaterOrEqual', value.getStartDate()),
                    this.makeRule(metadata && metadata.MDEndFieldName || 'FinishDate', 'LessOrEqual', value.getEndDate())
                ].filter(function (node) {
                    return !Ext.isEmpty(node);
                });

                return this.makeNode('and', nodes);
            } else if (value instanceof App.extfilter.core.ValueSearchList) {
                value = value.toString();
            }

            return {
                property: property,
                operation: operation,
                value: value
            };
        }

        return null;
    },

    makeNode: function (operator, rules) {
        if (!Ext.isEmpty(operator) && !Ext.isEmpty(rules)) {
            return {
                operator: operator,
                rules: rules
            };
        }

        return null;
    },

    updateFromFilter: function (filter) {
        console.warn('This model cannot be updated from filter');
    },

    getView: function () {
        return Ext.widget('extselectionfilter', this);
    },

    getFields: function (isDefault) {
        var model = this.getModel();

        var sortedFields = model.getFields().sort(function (f, s) {
            if (f.originalIndex !== undefined && - s.originalIndex !== undefined) {
                return f.originalIndex - s.originalIndex;
            }
            else {
                return 0;
            }
        });

        return sortedFields.filter(function (field) {
            return (!isDefault || field.isDefault) && ((Ext.isFunction(field.hidden) && !field.hidden()) || !field.hidden);
        }).map(function (field) {
            return field.name;
        });
    },

    getSelectedFields: function () {
        return this.entries.getRange().map(function (filter) {
            return filter.get('id');
        });
    },

     selectFields: function (newNames) {
        var currentNames = this.getSelectedFields(),
            toInsert = Ext.Array.difference(newNames, currentNames),
            toRemove = Ext.Array.difference(currentNames, newNames);

        
        if (toInsert.length < 20) {
            this.add(this.createFilterEntries(toInsert));
        } else {
            this.entries.addAll(this.createFilterEntries(toInsert));
            this.addFull(this);
        }
        if (toRemove.length < 20) {
            this.remove(toRemove.map(function (name) {
                return this.entries.getByKey(name);
            }, this));  
        } else {
            this.entries.removeAll(toRemove.map(function (name) {
                return this.entries.getByKey(name);
            }, this));
            this.removeFull(this);
        }
    },

    getFilterEntries: function () {
        return this.entries.getRange();
    },

    createFilterEntries: function (names) {
        var model = this.getModel(),
            modelClassFullName = Ext.getClassName(model),
            modelName = App.Util.getClassNameWithoutNamespace(modelClassFullName),
            moduleName = App.Util.getSubdirectory(modelClassFullName),
            fields = model.getFields().reduce(function (result, field) {
                result[field.name] = field;
                return result;
            }, {});

        return names.map(function (fieldName) {
            var field = fields[fieldName];
            var val = field.defaultFilterConfig ? field.defaultFilterConfig.value : undefined;
            var op = field.defaultFilterConfig ? field.defaultFilterConfig.operation : undefined;

            if (field.viewTree) {
                var fsearchValueDisplayField = field.defaultFilterConfig.valueField;
            } else {
                var fsearchValueDisplayField = field.mapping ? field.mapping.split('.')[field.mapping.split('.').length - 1] : fieldName;
            }
            var entryConfig = App.extfilter.core.ConfigSource.getEntryConfig(model, field, fsearchValueDisplayField)

            if (field.hasOwnProperty('extendedFilterEntry')) {
                console.warn('The "extendedFilterEntry" configuration no longer support')
            }
           
            return Ext.create('App.extfilter.core.SelectionFilterEntry', model, {
                id: fieldName,
                name: l10n.ns(moduleName, modelName).value(fieldName),
                field: field.mapping || fieldName,
                fieldType: field.type.type,
                value: val,
                operation: op,
                metadata: field.metadata
            }, entryConfig);
        }, this);
    },

    onClear: function () {
        this.init();
    }

});