Ext.define('App.util.core.ExtendedCheckboxModel', {
    extend: 'Ext.selection.CheckboxModel',
    alias: 'selection.extcheckboxmodel',
    mode: 'SINGLE',
    checkOnly: true,
    checkedRows: null,
    showHeaderCheckbox: true,
    selectAllRecordsCallback: null,
    deselectAllRecordsCallback: null,

    constructor: function (cfg) {
        this.callParent(arguments);
        this.checkedRows = new Ext.util.MixedCollection(null, this.getSelectionId);
    },

    getHeaderConfig: function () {
        var me = this;
        var sourceCfg = this.callParent(arguments);
        var cfg = {
            hideable: true,
            hidden: true,
            flex: 0,
            minWidth: 28,
            width: 28,
            filter: false,
            padding: '0 4 3 4',
            cls: 'select-all-header',
            configurable: false
        };
        var result = Ext.apply(sourceCfg, cfg);
        return result;
    },


    onHeaderClick: function (headerCt, header, e) {
        var grid = header.up('directorygrid'),
            store = grid.getStore();

        if (store.getTotalCount() !== 0) {
            if (header.isCheckerHd) {
                e.stopEvent();
                var me = this;
                if (me.allSelected) {
                    me.deselectAllRecords(header);
                    me.allSelected = false
                } else {
                    me.selectAllRecords(header);
                    me.allSelected = true
                }
                delete me.preventFocus;
            }
        }
    },

    addCheckbox: function (view, initial) {
        var me = this,
            checkbox = me.injectCheckbox,
            headerCt = view.headerCt;

        // Preserve behaviour of false, but not clear why that would ever be done.
        if (checkbox !== false) {
            if (checkbox == 'first') {
                checkbox = 0;
            } else if (checkbox == 'last') {
                checkbox = headerCt.getColumnCount();
            }
            Ext.suspendLayouts();
            headerCt.add(checkbox, me.getHeaderConfig());
            Ext.resumeLayouts();
        }

        if (initial !== true) {
            view.refresh();
        }
    },

    bindComponent: function (view) {
        var me = this;

        me.callParent(arguments);

        view.on({
            refresh: me.updateCheckedRows,
            scope: me
        });

        if (view.bufferedRenderer) {
            view.on({
                addverticalscroll: function (view) {
                    view.on({
                        scroll: me.updateCheckedRows,
                        element: 'jspContainer',
                        scope: me
                    });
                }
            });
        }
    },

    selectAllRecords: function (btn) {
        var me = this,
            win = btn.up('selectorwindow'),
            grid = win ? win.down('directorygrid') : btn.up('directorygrid'); // Если базовый грид с мультиселектом (не в отдельном окне)

        if (grid) {
            var store = grid.getStore(),
                selModel = grid.getSelectionModel(),
                recordsCount = store.getTotalCount();

            if (recordsCount > 0) {
                grid.setLoading(true);
                store.getRange(0, recordsCount, {
                    callback: function () {
                        if (recordsCount > 0) {
                            selModel.checkRows(store.getRange(0, recordsCount));

                            if (me.selectAllRecordsCallback) {
                                me.selectAllRecordsCallback();
                            }

                            grid.setLoading(false);
                        }
                        if (win) {
                            win.down('#select')[selModel.hasChecked() ? 'enable' : 'disable']();
                        }
                    }
                });
            }
        }
    },

    deselectAllRecords: function (btn) {
        var me = this,
            win = btn.up('selectorwindow'),
            grid = win ? win.down('directorygrid') : btn.up('directorygrid');

        if (grid) {
            var store = grid.getStore(),
                selModel = grid.getSelectionModel(),
                recordsCount = store.getTotalCount(),
                selectedRecords = recordsCount > 0 ? store.getRange(0, recordsCount) : [];

            if (recordsCount > 0) {
                selModel.uncheckRows(selectedRecords);

                if (me.deselectAllRecordsCallback) {
                    me.deselectAllRecordsCallback();
                }
            }
            if (win) {
                win.down('#select')[selModel.hasChecked() ? 'enable' : 'disable']();
            }
        }
    },

    loadRecords: function (grid, callback) {
        grid.setLoading(true);
        var store = grid.getStore();
        //var proxy = store.getProxy();

        store.addListener('load', function (store, records) {
            callback.call(store, records);
            grid.setLoading(false);
        });

        store.load();
        /*
        var resource = proxy.resourceName;
        var query = breeze.EntityQuery
            .from(resource)
            .using(Ext.ux.data.BreezeEntityManager.getEntityManager());
        query = App.util.core.Util.buildBreezeQuery(query, store)
            .using(Ext.ux.data.BreezeEntityManager.getEntityManager())
            .execute()
            .then(function (response) {
                grid.setLoading(false);
                var reader = proxy.getReader();
                var data = reader.read(response);
                if (data.success) {
                    callback.call(store, data.records);
                } else {
                    App.Notify.pushError('Ошибка при выделении всех записей');
                }
            })
            .fail(function (data) {
                grid.setLoading(false);
                App.Notify.pushError('Ошибка при выделении всех записей');
            });
        */
    },

    processSelection: function (view, record, item, index, e) {
        var me = this,
            checker = e.getTarget(me.checkSelector),
            mode;

        if (checker) {
            if (record) {
                if (!me.isChecked(record)) {
                    me.checkRows(record);
                } else {
                    me.checkedRows.remove(record);
                    me.fireEvent('unchecked', record);
                    Ext.fly(item).removeCls(Ext.baseCSSPrefix + 'grid-row-checked');
                }
            }

            me.fireEvent('selectionchange', me, me.getSelection());
        } else {
            me.selectWithEvent(record, e);
        }
    },

    onModelIdChanged: function (store, model, oldId, newId, oldInternalId) {
        this.callParent(arguments);
        this.checkedRows.updateKey(oldInternalId, newId);
    },

    updateCheckedRows: function () {
        var toBeChecked = this.checkedRows.filterBy(function (item) {
            return this.store.indexOf(item) !== -1;
        }, this);
        this.checkRows(toBeChecked.getRange());
    },

    getCheckedRows: function () {
        return this.checkedRows.getRange();
    },

    isChecked: function (record) {
        return this.checkedRows.contains(record);
    },

    hasChecked: function () {
        return this.checkedRows.getCount() > 0;
    },

    checkRows: function (records) {
        var me = this;

        if (records) {
            records = Ext.Array.from(records);
            records.forEach(function (item) {
                var el = me.view.getNode(item);

                if (!me.isChecked(item)) {
                    me.checkedRows.add(item);
                    me.fireEvent('checked', item);
                }

                if (el) {
                    Ext.fly(el).addCls(Ext.baseCSSPrefix + 'grid-row-checked');
                }
            }, me);
        }
    },

    uncheckRows: function (records) {
        var me = this;

        if (records) {
            records = Ext.Array.from(records);
            records.forEach(function (item) {
                var el = me.view.getNode(item);

                if (me.isChecked(item)) {
                    me.checkedRows.remove(item);
                    me.fireEvent('unchecked', item);
                }

                if (el) {
                    Ext.fly(el).removeCls(Ext.baseCSSPrefix + 'grid-row-checked');
                }
            }, me);
        }
    },

    getSelection: function () {
        var range = this.callParent(),
            store = this.getStore();

        if (Ext.isEmpty(range)) {
            return range;
        }

        return range.map(function (item) {
            return store.getById(item.get(item.idProperty)) || item;
        });
    }
});