Ext.define('App.view.core.common.SearchComboBox', {
    extend: 'Ext.form.field.ComboBox',
    alias: 'widget.searchcombobox',

    trigger1Cls: Ext.baseCSSPrefix + 'form-arrow-trigger',
    trigger2Cls: Ext.baseCSSPrefix + 'form-search-trigger',
    trigger3Cls: Ext.baseCSSPrefix + 'form-clear-trigger',

    multiSelect: false,
    editable: false,
    autoSelect: false,
    forceSelection: true,

    // Предполагается, что виджет, указанный в selectorWidget, 
    // является наслендиком Ext.grid.Panel.
    selectorWidget: undefined,
    isSearchField: true,
    needUpdateMappings: false,

    defaultStoreState: null,

    initComponent: function () {
        this.store.statefulFilters = true;
        this.callParent(arguments);
    },

    onBindStore: function (store, initial) {
        this.callParent(arguments);

        if (this.window) {
            this.getGrid().bindStore(store, initial);
        }
    },

    onTrigger2Click: function () {
        var window = this.createWindow();

        if (window) {
            window.show();
            this.getStore().load();
        }
    },

    onTrigger3Click: function () {
        this.clearValue();
    },

    onSelectionChange: function (selModel) {
        var window = this.window,
            hasSelection = selModel.hasSelection();

        if (window) {
            window.down('#select')[hasSelection ? 'enable' : 'disable']();
        }
    },

    onSelectButtonClick: function (button) {
        var window = this.window,
            selModel = window.down(this.selectorWidget).down('grid').getSelectionModel(),
            record = selModel.hasSelection() ? selModel.getSelection()[0] : null;

        this.setValue(record);
        //if (this.needUpdateMappings) {
        //    this.updateMappingValues(record);
        //}
        this.fireEvent('select', this, record);

        window.close();
    },

    updateMappingValues: function (record) {
        var f = this.up('form');
        if (f && this.mapping) {
            var form = f.getForm();
            var rec = form.getRecord();
            for (var i in this.mapping) {
                var map = this.mapping[i];
                var val = record.get(map.from);
                var field = form.findField(map.to);
                if (field) {
                    field.setValue(val);
                }
                rec.set(map.to, val);
            }
        }
    },

    onCancelButtonClick: function (button) {
        this.window.close();
    },

    onWindowClose: function () {
        if (this.defaultStoreState) {
            this.getStore().applyState(this.defaultStoreState);
            this.getStore().load();
        }
    },

    createWindow: function () {
        var window,
            store = this.getStore();

        var selectorWidgetConfig = this.selectorWidgetConfig || {};
        selectorWidgetConfig.xtype = this.selectorWidget;

        window = this.window = Ext.widget('selectorwindow', {
            title: l10n.ns('core').value('selectorWindowTitle'),
            items: [selectorWidgetConfig]
        });

        if (!this.defaultStoreState) {
            this.defaultStoreState = store.getState() || {};
            Ext.applyIf(this.defaultStoreState, {
                sorters: [],
                filters: []
            });
        }

        this.getGrid().bindStore(store);
        this.addHandlers(window);

        return window;
    },

    addHandlers: function (window) {
        window.down('#select').on('click', this.onSelectButtonClick, this);
        window.down('#cancel').on('click', this.onCancelButtonClick, this);
        window.on('close', this.onWindowClose, this);
        this.getGrid().on('selectionchange', this.onSelectionChange, this);
        this.getGrid().getStore().on({
            single: true,
            scope: this,
            load: this.onFirstLoad
        });
    },

    clearValue: function () {
        this.setValue(null);
    },

    setValue: function (value, doSelect) {
        var record = Ext.Array.from(value)[0],
            store = this.getStore(),
            processedValue,
            displayValue, idx;

        if ((!record || !record.isModel) && value) {
            idx = store.findExact(this.valueField, value.value || value);
            record = idx !== -1 ? store.getAt(idx) : null;
        }

        if (record) {
            processedValue = record.get(this.valueField);
            displayValue = record.get(this.displayField);
        } else {
            if (value) {
                if (Ext.isObject(value)) {
                    var data = {};
                    data[this.valueField] = value.value;
                    data[this.displayField] = value.display;
                    this.displayTplData = Ext.Array.from(data);
                    this.value = value.value;
                    this.setRawValue(value.display);
                } else {
                    store.on({
                        scope: this,
                        single: true,
                        load: function () {
                            this.setValue(value);
                        }
                    });
                }
                return this;
            }
        }

        this.displayTplData = record ? Ext.Array.from(record.data) : record;
        this.lastSelection = this.valueModels = record ? Ext.Array.from(record) : record;
        this.value = processedValue;
        this.setRawValue(displayValue);
        this.record = record;
        this.checkChange();
        this.afterSetValue(record);

        if (doSelect !== false) {
            this.syncSelection();
        }

        return this;
    },

    getRecord: function () {
        var ret = this.record;
        if (!ret || !ret.isModel) {
            var idx = this.store.findExact(this.valueField, this.value);
            ret = idx !== -1 ? this.store.getAt(idx) : null;
        }
        return ret;
    },

    getValue: function () {
        return this.value;
    },

    getSubmitData: function () {
        var data = this.callParent(arguments);
        return Ext.applyIf(data, this.getRecordData());
    },

    getModelData: function () {
        var data = this.callParent(arguments);
        return Ext.applyIf(data, this.getRecordData());
    },

    getRecordData: function () {
        var mapping = Ext.Array.from(this.mapping),
            record = this.record, recData;

        recData = record && record.getData();

        return mapping.reduce(function (data, map) {
            if (recData && recData.hasOwnProperty(map.from)) {
                data[map.to] = recData[map.from];
            }
            return data;
        }, {});
    },

    getGrid: function () {
        return this.window.down(this.selectorWidget).down('grid');
    },

    onFirstLoad: function (store) {
        var selModel = this.getGrid().getSelectionModel(),
            record = this.getRecord();

        if (record) {
            this.getGrid().getSelectionModel().select(record);
        }
    },

    onExpand: function () {
        this.callParent(arguments);
        this.syncSelection();
    },

    afterSetValue: function (record) {
        if (this.needUpdateMappings) {
            this.updateMappingValues(record);
        }
    }
});