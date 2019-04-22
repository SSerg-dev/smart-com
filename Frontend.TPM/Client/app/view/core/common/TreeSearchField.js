Ext.define('App.view.tpm.nonenego.TreeSearchField', {
    extend: 'Ext.form.field.Trigger',
    alias: 'widget.treesearchfield',
    mixins: ['Ext.util.Bindable'],

    trigger1Cls: Ext.baseCSSPrefix + 'form-search-trigger',
    trigger2Cls: Ext.baseCSSPrefix + 'form-clear-trigger',

    // Предполагается, что виджет, указанный в selectorWidget, 
    // является наслендиком Ext.grid.Panel.
    selectorWidget: undefined,
    editable: false,
    isSearchField: true,
    needUpdateMappings: false,

    defaultStoreState: null,

    initComponent: function () {
        this.store.autoLoad = false;
        this.store.statefulFilters = true;
        this.bindStore(this.store);
        this.callParent(arguments);
    },

    onDestroy: function () {
        this.bindStore(null);
        this.callParent();
    },

    onBindStore: function (store, initial) {
        var picker = this.picker;

        //if (picker) {
        //    picker.down(this.selectorWidget).down('grid').bindStore(store, initial);
        //}
    },

    onTrigger1Click: function () {
        var picker = this.createPicker();

        if (picker) {
            picker.show();
            var selectorWidget = picker.down(this.selectorWidget),
                elementsToHide = selectorWidget.query('[hierarchyOnly=true]');
            elementsToHide.forEach(function (el) { el.hide(); });
        }
    },

    onTrigger2Click: function () {
        this.clearValue();
    },

    onSelectionChange: function (selModel) {
        var picker = this.picker,
            hasSelection = selModel.hasSelection();

        if (picker) {
            picker.down('#select')[hasSelection ? 'enable' : 'disable']();
        }
    },

    onSelectButtonClick: function (button) {
        var picker = this.picker,
            selModel = picker.down(this.selectorWidget).down('basetreegrid').getSelectionModel(),
            record = selModel.hasSelection() ? selModel.getSelection()[0] : null;

        this.setValue(record);
        if (this.needUpdateMappings) {
            this.updateMappingValues(record);
        }
        this.afterSetValue(record);
        picker.close();
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
        this.picker.close();
    },

    afterPickerCreate: function (scope) {

    },

    createPicker: function () {
        var picker,
            store = this.getStore();

        var selectorWidgetConfig = this.selectorWidgetConfig || {};
        selectorWidgetConfig.xtype = this.selectorWidget;

        picker = this.picker = Ext.widget('selectorwindow', {
            title: l10n.ns('core').value('selectorWindowTitle'),
            items: [selectorWidgetConfig],
            width: 900,
            height: 650,
            minWidth: 900,
            minHeight: 650,
            autoScroll: false
        });

        this.addHandlers(picker);
        this.afterPickerCreate(this);
        return picker;
    },

    addHandlers: function (picker) {
        var me = this;

        picker.down(this.selectorWidget).down('basetreegrid').on('selectionchange', this.onSelectionChange, this);
        picker.down('#select').on('click', this.onSelectButtonClick, this);
        picker.down('#cancel').on('click', this.onCancelButtonClick, this);
    },

    clearValue: function () {
        this.setValue(null);
    },

    setValue: function (value) {
        var record = value,
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

        this.value = processedValue;
        this.setRawValue(displayValue);
        this.record = record;
        this.checkChange();

        return this;
    },

    afterSetValue: function (record) {

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
    }
});