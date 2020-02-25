Ext.define('App.extfilter.core.ConfigSource', {
    statics: {

        getEntryConfig: function (model, field, fsearchValueDisplayField) {
            var modelClassName = Ext.getClassName(model),
                modelName = App.Util.getClassNameWithoutNamespace(modelClassName),
                fieldName = field.defaultFilterConfig ? field.defaultFilterConfig.valueField || field.name : field.name,
                fieldType = field.type ? field.type.type : null,
                entityName = field.breezeEntityType,
                moduleName = App.Util.getSubdirectory(modelClassName),
                customLocale = field.localeConfig,
                tree = field.tree,
                viewTree = field.viewTree,
                timeZone = field.timeZone;

            var cfg = field.filterOperationsConfig ? field.filterOperationsConfig : {};
            // Для кастомной локализации булевых полей
            if (fieldType === 'bool' && customLocale) {
                cfg = Ext.Object.merge(cfg, {
                    editors: {
                        boolean: {
                            extend: 'booleancombobox',
                            store: {
                                extend: 'Ext.data.Store',
                                fields: ['id', 'text'],
                                data: [
                                    { id: true, text: l10n.ns(customLocale.module, customLocale.namespace).value(customLocale.trueProperty) },
                                    { id: false, text: l10n.ns(customLocale.module, customLocale.namespace).value(customLocale.falseProperty) }
                                ]
                            },
                        }
                    }
                })
            }
            if (fieldType === 'date') {
                if (timeZone !== undefined && timeZone !== null) {
                    cfg = Ext.Object.merge(cfg, {
                        editors: {
                            date: {
                                xtype: 'datefieldtimezone',
                                timeZone: timeZone,
                                editable: false
                            }
                        },
                        values: {
                            date: {
                                list: {
                                    itemRenderer: function (value) {
                                        if (!Ext.isEmpty(value) && Ext.isDate(value)) {
                                            return Ext.Date.format(value, 'd.m.Y');
                                        }

                                        if ((value.from && Ext.isDate(value.from)) || (value.to && Ext.isDate(value.to))) {
                                            return {
                                                from: Ext.Date.format(value.from, 'd.m.Y'),
                                                to: Ext.Date.format(value.to, 'd.m.Y')
                                            }
                                        }

                                        return value;
                                    }
                                }
                            }
                        }
                    });
                }
                else {
                    cfg = Ext.Object.merge(cfg, {
                        editors: {
                            date: {
                                editable: false
                            }
                        },
                        values: {
                            date: {
                                list: {
                                    itemRenderer: function (value) {
                                        if (!Ext.isEmpty(value) && Ext.isDate(value)) {
                                            return Ext.Date.format(value, 'd.m.Y');
                                        }

                                        if ((value.from && Ext.isDate(value.from)) || (value.to && Ext.isDate(value.to))) {
                                            return {
                                                from: Ext.Date.format(value.from, 'd.m.Y'),
                                                to: Ext.Date.format(value.to, 'd.m.Y')
                                            }
                                        }

                                        return value;
                                    }
                                }
                            }
                        }
                    });
                }
            }

            if (fieldType === 'float') {
                cfg = Ext.Object.merge(cfg, {
                    editors: {
                        float: {
                            decimalPrecision: 20,
                            decimalSeparator: '.',
                            minValue: 0
                        }
                    }
                });
            }

            if (fieldType === 'int') {
                cfg = Ext.Object.merge(cfg, {
                    editors: {
                        int: {
                            minValue: 0
                        }
                    }
                });
            }

            var isHandler = Ext.Array.some(['HistoricalLoopHandler', 'LoopHandler', 'SingleLoopHandler', 'UserLoopHandler'], function (item) {
                return modelName === item && (fieldName === 'LastExecutionDate' || fieldName === 'NextExecutionDate');
            });

            var isMapping = Ext.Array.some(['DeletedMapping', 'HistoricalMapping', 'Mapping', 'DeletedMappingExclusion', 'HistoricalMappingExclusion', 'MappingExclusion'], function (item) {
                return modelName === item || fieldName === 'CreateDate';
            });

            var isForecast = Ext.Array.some(['ForecastMappedAggregatedActual', 'ForecastMappedAggregated', 'ForecastMapped', 'Forecast'], function (item) {
                return modelName === item || fieldName === 'CreateDate' || fieldName === 'StartDate' || fieldName === 'FinishDate';
            });

            if (Ext.String.startsWith(modelName, 'Deleted') || Ext.String.startsWith(modelName, 'Historical') || isHandler || isMapping || isForecast) {
                if (fieldType === 'date') {
                    if (timeZone !== undefined && timeZone !== null) {
                        cfg = Ext.Object.merge(cfg, {
                            editors: {
                                date: {
                                    xtype: 'datefieldtimezone',
                                    timeZone: timeZone,
                                }
                            },
                            values: {
                                date: {
                                    list: {
                                        itemRenderer: function (value) {
                                            if (!Ext.isEmpty(value) && Ext.isDate(value)) {
                                                return Ext.Date.format(value, 'd.m.Y');
                                            }

                                            return value;
                                        }
                                    }
                                }
                            }
                        });
                    }
                    else {
                        cfg = Ext.Object.merge(cfg, {
                            editors: {
                                date: {
                                    xtype: 'datetimefield'
                                }
                            },
                            values: {
                                date: {
                                    list: {
                                        itemRenderer: function (value) {
                                            if (!Ext.isEmpty(value) && Ext.isDate(value)) {
                                                return Ext.Date.format(value, 'd.m.Y');
                                            }

                                            return value;
                                        }
                                    }
                                }
                            }
                        });
                    }
                }
            }

            if (Ext.String.startsWith(modelName, 'Historical')) {
                if (fieldType === 'string') {
                    cfg = Ext.Object.merge(cfg, {
                        defaultOperations: {
                            string: 'Equals'
                        },
                        allowedOperations: {
                            string: ['Equals', 'NotEqual', 'In', 'IsNull', 'NotNull']
                        }
                    });

                    if (fieldName === '_Operation') {
                        cfg = Ext.Object.merge(cfg, {
                            editors: {
                                string: {
                                    xtype: 'combobox',
                                    valueField: 'id',
                                    store: {
                                        type: 'operationtypestore'
                                    }
                                }
                            },
                            values: {
                                string: {
                                    list: {
                                        itemRenderer: function (value) {
                                            if (!Ext.isEmpty(value) && Ext.isString(value)) {
                                                return l10n.ns('core', 'enums', 'OperationType').value(value);
                                            }

                                            return value;
                                        }
                                    }
                                }
                            }
                        });
                    }
                }
            }

            if (fieldName === 'MarsPeriod') {
                cfg = Ext.Object.merge(cfg, {
                    editors: {
                        string: {
                            xtype: 'marsdatefield'
                        }
                    },
                    defaultOperations: {
                        string: 'Equals'
                    },
                    allowedOperations: {
                        string: ['Equals', 'NotEqual', 'In', 'StartsWith', 'Between']
                    }
                });
            }

            if (entityName) {
                var modelClassName = Ext.String.format('App.model.{0}.{1}.{2}', moduleName, entityName.toLowerCase(), entityName);
                if (tree) {
                    cfg = Ext.Object.merge(cfg, {
                        editors: {
                            string: {
                                xtype: 'treefsearchfield',
                                trigger2Cls: '',
                                selectorWidget: entityName.toLowerCase(),
                                valueField: fsearchValueDisplayField,
                                displayField: fsearchValueDisplayField,
                                store: {
                                    model: modelClassName,
                                    autoLoad: false,
                                    root: {}
                                }
                            }
                        },
                        valueTypes: {
                            'In': ['searchlist']
                        }
                    });
                } else {
                    cfg = Ext.Object.merge(cfg, {
                        editors: {
                            string: {
                                xtype: 'fsearchfield',
                                trigger2Cls: '',
                                selectorWidget: entityName.toLowerCase(),
                                valueField: fieldName,
                                displayField: fieldName,
                                store: {
                                    type: 'directorystore',
                                    model: modelClassName,
                                    extendedFilter: {
                                        xclass: 'App.ExtFilterContext',
                                        supportedModels: [{
                                            xclass: 'App.ExtSelectionFilterModel',
                                            model: modelClassName,
                                            modelId: 'efselectionmodel'
                                        }, {
                                            xclass: 'App.ExtTextFilterModel',
                                            modelId: 'eftextmodel'
                                        }]
                                    }
                                }
                            }
                        },
                        valueTypes: {
                            'In': ['searchlist']
                        }
                    });
                }
            }

            return cfg;
        }
    }
});