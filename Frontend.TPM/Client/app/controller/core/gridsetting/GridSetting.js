Ext.define('App.controller.core.gridsetting.GridSetting', {
    extend: 'Ext.app.Controller',

    refs: [{
        ref: 'gridSettingsWindow',
        selector: 'gridsettingswindow',
        xtype: 'gridsettingswindow',
        autoCreate: true
    }],

    init: function () {
        this.listen({
            component: {
                'customheadermenu #gridsettings': {
                    click: this.onGridSettingsBtnClick
                },
                'gridsettingswindow #save': {
                    click: this.onSaveBtnClick
                },
                'gridsettingswindow #applydefault': {
                    click: this.onApplyDefaultBtnClick
                },
                '#hiddencolumns': {
                    selectionchange: this.onHiddenColumnsSelChange
                },
                '#visiblecolumns': {
                    selectionchange: this.onVisibleColumnsSelChange
                },
                'colsettingstoolbar #add': {
                    click: this.onAddBtnClick
                },
                'colsettingstoolbar #remove': {
                    click: this.onRemoveBtnClick
                },
                'colsettingstoolbar #up': {
                    click: this.onUpBtnClick
                },
                'colsettingstoolbar #down': {
                    click: this.onDownBtnClick
                },
                'colsettingstoolbar #clearSort': {
                    click: this.onClearSortBtnClick
                },
                'colsettingstoolbar #width': {
                    change: this.onWidthChange
                }
            }
        });
    },

    onGridSettingsBtnClick: function (button) {
        var grid = button.up('menu').ownerButton
            .up('combineddirectorypanel')
            .down('directorygrid');

        if (grid && grid.isConfigurable) {
            var state = grid.getCurrentState(),
                window = this.getGridSettingsWindow();

            this.loadGridsData(state, window);
            window.ownerGrid = grid;
            window.show();
        }
    },

    loadGridsData: function (state, window) {
        var columns = Ext.Array.clone(state.columns),
            hiddenColumns = [],
            visibleColumns = [];

        if (state.storeState && Ext.isArray(state.storeState.sorters)) {
            columns.forEach(function (col) {
                var sort = Ext.Array.findBy(state.storeState.sorters, function (item) {
                    return item.property === col.dataIndex;
                });
                if (sort) {
                    col.sort = sort.direction;
                }
            });
        }

        columns.forEach(function (col, index) {
            if (Ext.isEmpty(col.dataIndex) || col.configurable === false) {
                return;
            }

            if (col.hidden === true) {
                hiddenColumns.push(col);
            } else {
                visibleColumns.push(col);
            }
        });

        window.down('#hiddencolumns').getStore().loadData(hiddenColumns);
        window.down('#visiblecolumns').getStore().loadData(visibleColumns);
    },

    onSaveBtnClick: function (button) {
        var window = button.up('window'),
            hiddenColumns = window.down('#hiddencolumns').getStore().getRange(),
            visibleColumns = window.down('#visiblecolumns').getStore().getRange(),
            ownerGrid = window.ownerGrid,
            xtype = ownerGrid.up('combineddirectorypanel').getXType(),
            currentColumns = ownerGrid.getCurrentState().columns,
            gridSettings = {
                columns: [],
                sort: []
            };

        hiddenColumns = hiddenColumns.map(function (rec) {
            var cfg = rec.getData();
            cfg.hidden = true;
            return cfg;
        });

        visibleColumns = visibleColumns.map(function (rec) {
            var cfg = rec.getData();
            cfg.hidden = false;

            if (!Ext.isEmpty(rec.get('width'))) {
                cfg.width = rec.get('width');
            } else {
                cfg.flex = 1;
            }

            return cfg;
        });

        visibleColumns.concat(hiddenColumns)
            .concat(currentColumns.filter(function (col) {
                return Ext.isEmpty(col.dataIndex) || col.configurable === false;
            }))
            .sort(function (col1, col2) {
                return col1.index - col2.index;
            })
            .forEach(function (col) {
                if (!Ext.isEmpty(col.sort)) {
                    gridSettings.sort.push({
                        property: col.dataIndex,
                        direction: col.sort
                    });
                }

                var cfg = {
                    dataIndex: col.dataIndex,
                    hidden: col.hidden,
                    autoWidth: col.autoWidth
                };

                if (!Ext.isEmpty(col.width)) {
                    cfg.width = col.width;
                } else if (!Ext.isEmpty(col.flex)) {
                    cfg.flex = col.flex;
                }

                gridSettings.columns.push(cfg);
            });

        window.setLoading(l10n.ns('core').value('savingText'));
        App.UserInfo.setGridSettings(xtype, null, gridSettings)
            .then(function () {
                window.close();
                ownerGrid.updateColumns(xtype);
            })
            .fail(function () {
                window.setLoading(false);
            });
    },

    onApplyDefaultBtnClick: function (button) {
        var state = button.up('window').ownerGrid.getDefaultState();
        this.loadGridsData(state, this.getGridSettingsWindow());
    },

    onHiddenColumnsSelChange: function (selModel) {
        var toolbar = selModel.view.up('window').down('#movingToolbar');
        this.updateButtonsState(toolbar, '#add', selModel.hasSelection());
    },

    onVisibleColumnsSelChange: function (selModel, selected) {
        var window = selModel.view.up('window');
        this.updateButtonsState(window, '#remove, #visibleColumnsToolbar button, #width', selModel.hasSelection());

        if (selModel.hasSelection()) {
            var toolbar = window.down('#visibleColumnsToolbar'),
                store = window.down('#visiblecolumns').getStore(),
                colCount = store.getCount(),
                index = store.indexOf(selected[0]),
                width = selected[0].get('autoWidth') ? null : selected[0].get('width');

            toolbar.down('#width').setValue(width);
            toolbar.down('#up').setDisabled(index < 1);
            toolbar.down('#down').setDisabled(index >= colCount - 1);
            window.down('#movingToolbar #remove').setDisabled(colCount <= 1);

            switch (selected[0].get('sort')) {
                case 'ASC':
                    toolbar.down('#sortAsc').toggle(true);
                    break;
                case 'DESC':
                    toolbar.down('#sortDesc').toggle(true);
                    break;
                default:
                    toolbar.down('#clearSort').toggle(true);
            }
        }
    },

    updateButtonsState: function (container, queryString, isEnabled) {
        container.query(queryString)
            .forEach(function (button) {
                button.setDisabled(!isEnabled);
            });
    },

    onAddBtnClick: function (button) {
        var window = button.up('window'),
            hiddenColumnsGrid = window.down('#hiddencolumns'),
            visibleColumnsGrid = window.down('#visiblecolumns'),
            selModel = hiddenColumnsGrid.getSelectionModel();

        if (selModel.hasSelection()) {
            var selRecord = selModel.getSelection()[0],
                model = visibleColumnsGrid.getStore().add(selRecord.getData());

            hiddenColumnsGrid.getStore().remove(selRecord);
            visibleColumnsGrid.getSelectionModel().select(model);
            this.updateColumnsIndexes(visibleColumnsGrid.getStore());
        }

        hiddenColumnsGrid.down('gridview')._refreshScroll();
        visibleColumnsGrid.down('gridview')._refreshScroll();
    },

    onRemoveBtnClick: function (button) {
        var window = button.up('window'),
            hiddenColumnsGrid = window.down('#hiddencolumns'),
            visibleColumnsGrid = window.down('#visiblecolumns'),
            selModel = visibleColumnsGrid.getSelectionModel(),
            colCount = visibleColumnsGrid.getStore().getCount();

        if (selModel.hasSelection() && colCount > 1) {
            var selRecord = selModel.getSelection()[0];
            selRecord.set('sort', null);
            var model = hiddenColumnsGrid.getStore().add(selRecord.getData());
            visibleColumnsGrid.getStore().remove(selRecord);
            hiddenColumnsGrid.getSelectionModel().select(model);
            this.updateColumnsIndexes(hiddenColumnsGrid.getStore());
        }

        hiddenColumnsGrid.down('gridview')._refreshScroll();
        visibleColumnsGrid.down('gridview')._refreshScroll();
    },

    updateColumnsIndexes: function (store) {
        var indexes = store.collect('index', true)
            .sort(function (i1, i2) {
                return i1 - i2;
            });

        store.each(function (rec, i) {
            rec.set('index', indexes[i]);
        });
    },

    onUpBtnClick: function (button) {
        var window = button.up('window'),
            visibleColumnsGrid = window.down('#visiblecolumns');

        this.moveSelectedRecord(visibleColumnsGrid, -1);
    },

    onDownBtnClick: function (button) {
        var window = button.up('window'),
            visibleColumnsGrid = window.down('#visiblecolumns');

        this.moveSelectedRecord(visibleColumnsGrid, 1);
    },

    moveSelectedRecord: function (grid, direction) {
        var toolbar = grid.up('window').down('#visibleColumnsToolbar'),
            selModel = grid.getSelectionModel(),
            record = selModel.getSelection()[0],
            store = grid.getStore();

        if (!record) {
            return;
        }

        var index = store.indexOf(record);

        if (direction < 0) {
            index--;
            if (index < 0) {
                return;
            }
        } else {
            index++;
            if (index >= store.getCount()) {
                return;
            }
        }

        var replaceableRecord = store.getAt(index),
            oldColIndex = record.get('index');

        record.set('index', replaceableRecord.get('index'));
        replaceableRecord.set('index', oldColIndex);

        store.remove(record);
        store.insert(index, record);
        selModel.select(record);

        toolbar.down('#up').setDisabled(index == 0);
        toolbar.down('#down').setDisabled(index == store.getCount() - 1);
    },

    onClearSortBtnClick: function (button) {
        var window = button.up('window'),
            visibleColumnsGrid = window.down('#visiblecolumns'),
            selModel = visibleColumnsGrid.getSelectionModel();

        if (selModel.hasSelection()) {
            selModel.getSelection()[0].set('sort', null);
        }
    },

    onWidthChange: function (field, newValue) {
        var grid = field.up('window').down('#visiblecolumns'),
            selModel = grid.getSelectionModel();

        if (selModel.hasSelection() && field.isValid()) {
            var record = selModel.getSelection()[0];
            record.set('width', newValue);
            record.set('autoWidth', false);
        }
    }
});