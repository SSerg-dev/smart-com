Ext.define('App.util.core.ConfigurableGrid', {
    isConfigurable: true,
    defaultState: {},
    currentState: null,
    suspendColResizeHandler: false,

    constructor: function (cfg) {
        if (cfg[0]) {
            var columns = cfg[0].columns;

            if (columns) {
                if (!Ext.isArray(columns)) {
                    var defaults = columns.defaults;
                    columns = Ext.Array.from(columns.items);

                    if (defaults) {
                        columns.forEach(function (item) {
                            Ext.applyIf(item, defaults);
                        });
                    }
                }

                var hasAdditionalColumn = this.getSelectionModel() instanceof Ext.selection.CheckboxModel;
                columns.forEach(function (item, index) {
                    item.index = index + (hasAdditionalColumn ? 1 : 0);
                    item.autoWidth = true;
                });
            }

            this.defaultState = {
                columns: columns,
                storeState: {
                    sorters: cfg[0].store ? cfg[0].store.sorters : null
                }
            };
        }

        this.addHandlers();
    },

    addHandlers: function () {
        this.on('columnresize', function (ct, column, width) {
            if (!this.suspendColResizeHandler) {
                column.autoWidth = false;
            }
        }, this);

        this.on('beforehide', function (ct, column, width) {
            this.saveCurrentState();
        }, this);

        this.on('show', function (ct, column, width) {
            this.resetSavedState();
        }, this);
    },

    initColumnsState: function (xtype, mode) {
        var config = App.UserInfo.getGridSettings(!Ext.isEmpty(xtype) ? xtype : this.getXType());

        if (!Ext.isEmpty(config)) {
            this.applyState({
                columns: config.columns
            });
            var store = this.getStore();
            //Часть кода добавления сортировки из метода стора applyState
            var hasSorters = !!store.sorters;
            if (hasSorters && config.sort) {
                store.sorters.clear();
                if (config.sort.length > 0) {
                    store.sorters.addAll(store.decodeSorters(config.sort));
                }
            }
            this.getColumnManager().getColumns().forEach(function (col) {
                var cfg = Ext.Array.findBy(config.columns, function (item) {
                    return item.dataIndex === col.dataIndex;
                });
                if (cfg) {
                    col.autoWidth = cfg.autoWidth;
                }
            });
        }
    },

    updateColumns: function (xtype, mode) {
        var config = App.UserInfo.getGridSettings(!Ext.isEmpty(xtype) ? xtype : this.getXType());

        if (Ext.isEmpty(config)) {
            return;
        }

        if (this.isVisible()) {
            this.updateColumnsInternal(config);
        } else {
            this.initColumnsState(xtype, mode);
            this.on({
                single: true,
                scope: this,
                show: function () {
                    this.getView().refresh();
                }
            });
        }

        if (this.currentState) {
            this.saveCurrentState();
            this.currentState.columns.forEach(function (column, index) {
                Ext.apply(column, config.columns[index]);
            });
        }
    },

    updateColumnsInternal: function (config) {
        var columns = config.columns;
        if (!Ext.isArray(columns)) {
            return;
        }

        Ext.suspendLayouts();

        var store = this.getStore(),
            defaultColumnsState = this.getDefaultState().columns,
            columnManager = this.getColumnManager(),
            count = columnManager.getColumns().length,
            i = 0,
            length = columns.length,
            c, col, columnState, index;

        for (c = 0; c < length; c++) {
            columnState = columns[c];

            for (index = count; index--;) {
                col = columnManager.getColumns()[index];

                //TODO: могут быть проблемы, если у нескольких колонок будет dataIndex = ''
                if (col.dataIndex === columnState.dataIndex) {
                    if (i !== index) {
                        this.headerCt.move(index, i);
                    }

                    if (columnState.hidden != null) {
                        col.setVisible(!columnState.hidden);

                        if (columnState.hidden && col.filter && filterbar) {
                            var indexForRemove = store.filters.findIndexBy(function (item) {
                                return item.property === col.dataIndex;
                            });

                            if (indexForRemove > -1) {
                                store.filters.removeAt(indexForRemove);
                            }
                        }
                    }

                    col.autoWidth = !Ext.isEmpty(columnState.autoWidth) ? columnState.autoWidth : defaultColumnsState.autoWidth;
                    if (!Ext.isEmpty(columnState.flex)) {
                        col.flex = columnState.flex;
                        col.setWidth(null);
                    } else if (columnState.autoWidth === true) {
                        var width = Ext.Array.findBy(defaultColumnsState, function (item) {
                            return item.dataIndex === columnState.dataIndex;
                        }).width;
                        col.setWidth(width);
                    } else if (!Ext.isEmpty(columnState.width)) {
                        col.flex = null;
                        col.setWidth(columnState.width);
                    }

                    ++i;
                    break;
                }
            }
        }

        this.suspendColResizeHandler = true;
        Ext.resumeLayouts(true);
        this.suspendColResizeHandler = false;

        var filterbar = this.getPlugin('filterbar');
        if (filterbar) {
            filterbar.resetup(this);
        }

        store.sorters.clear();
        store.sorters.addAll(store.decodeSorters(config.sort));
        store.load();
    },

    getCurrentState: function () {
        if (this.currentState) {
            return this.currentState;
        }

        var me = this,
            state = me.getState(),
            columns = me.getColumnManager().getColumns(),
            stateColumns = [];

        columns.forEach(function (col) {
            var cfg = {
                dataIndex: col.dataIndex,
                hidden: col.hidden,
                text: col.text,
                index: col.getIndex(),
                configurable: Ext.isEmpty(col.configurable) ? true : col.configurable,
                autoWidth: Ext.isEmpty(col.autoWidth) ? true : col.autoWidth
            };

            if (!Ext.isEmpty(col.flex)) {
                cfg.flex = col.flex;
            } else {
                var width = col.getWidth();
                cfg.width = width != 0 ? width : col.width;
            }

            stateColumns.push(cfg);
        });

        state = me.addPropertyToState(state, 'columns', stateColumns);
        return state;
    },

    saveCurrentState: function () {
        this.resetSavedState();
        this.currentState = this.getCurrentState();
    },

    resetSavedState: function () {
        this.currentState = null;
    },

    getDefaultState: function () {
        return this.defaultState;
    }
});