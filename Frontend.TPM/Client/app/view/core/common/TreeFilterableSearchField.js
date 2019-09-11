Ext.define('App.view.tpm.nonenego.TreeFilterableSearchField', {
    extend: 'Ext.form.field.Trigger',
    alias: 'widget.treefsearchfield',
    mixins: ['Ext.util.Bindable'],

    trigger1Cls: Ext.baseCSSPrefix + 'form-search-trigger',
    trigger2Cls: Ext.baseCSSPrefix + 'form-clear-trigger',

    multiSelect: false,
    delimiter: ';',
    valueField: null,

    // Предполагается, что виджет, указанный в selectorWidget, 
    // является наслендиком Ext.grid.Panel.
    selectorWidget: undefined,

    defaultStoreState: null,

    constructor: function () {
        this.callParent(arguments);
        this.addEvents('select');
    },

    initComponent: function () {
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
        //    this.getGrid().bindStore(store, initial);
        //}
    },

    onTrigger1Click: function () {
        var picker = this.createPicker();

        if (picker) {
            picker.show();
            var selectorWidget = picker.down(this.selectorWidget),
                elementsToHide = selectorWidget.query('[hierarchyOnly=true]');
            elementsToHide.forEach(function (el) { el.hide(); });

            if (this.multiSelect) {
                var treegrid = this.getTreeGrid();
                treegrid.checkedNodes = true;
                //запоминаем выбранные узлы, чтобы при повторном открытии дерева в расширенном фильтре выбрать эти узлы
                treegrid.checkedArray = this.getValue();
            }
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
            value = null;

        if (this.multiSelect) {
            value = this.getTreeGrid().getChecked();
        } else if (selModel.hasSelection()) {
            value = selModel.getSelection()[0];
        }

        this.setValue(value);
        this.fireEvent('select', this);
        picker.close();
    },

    onCancelButtonClick: function (button) {
        this.picker.close();
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

    afterPickerCreate: function (scope) {
    },

    addHandlers: function (picker) {
        picker.down(this.selectorWidget).down('basetreegrid').on('selectionchange', this.onSelectionChange, this);
        picker.down('#select').on('click', this.onSelectButtonClick, this);
        picker.down('#cancel').on('click', this.onCancelButtonClick, this);

        //this.getStore().on({
        //    single: true,
        //    scope: this,
        //    load: this.onFirstLoad
        //});
    },

    clearValue: function () {
        this.setValue(null);
    },

    setValue: function (value) {
        if (value && Ext.getClassName(value) !== 'App.extfilter.core.ValueSearchList') {
            value = Ext.create('App.extfilter.core.ValueSearchList', value, this.valueField, this.delimiter);
        }

        this.value = value;
        this.setRawValue(this.valueToRaw(value));
        this.checkChange();

        return this;
    },

    getValue: function () {
        var me = this,
            val = me.rawToValue(me.processRawValue(me.getRawValue()));

        if (Ext.isEmpty(val)) {
            return;
        }

        if (this.value) {
            this.value.update(val);
        } else {
            this.value = Ext.create('App.extfilter.core.ValueSearchList', val, this.valueField, this.delimiter);
        }

        return this.value.clone();
    },

    processRawValue: function (value) {
        return this.processStringValue(this.callParent(arguments));
    },

    processStringValue: function (value) {
        if (value && this.multiSelect) {
            value = Ext.Array.clean(Ext.Array.unique(value.split(this.delimiter)));
            if (value.length == 0) {
                value = null;
            }
        }

        return value;
    },

    valueToRaw: function (value) {
        return value ? value.toString(this.itemRenderer, this) : '';
    },

    didValueChange: function (newVal, oldVal) {
        newVal = Ext.getClassName(newVal) === 'App.extfilter.core.ValueSearchList'
            ? newVal.getValues()
            : Ext.Array.from(newVal);

        oldVal = Ext.getClassName(oldVal) === 'App.extfilter.core.ValueSearchList'
            ? oldVal.getValues()
            : Ext.Array.from(oldVal);

        return !Ext.Array.equals(newVal, oldVal);
    },

    getTreeGrid: function () {
        return this.picker.down(this.selectorWidget).down('basetreegrid');
    }
    /*
    onFirstLoad: function (store) {
        var selModel = this.getGrid().getSelectionModel(),
            rawValue = this.getValue();

        if (this.value) {
            var records = this.value.getRecords();

            if (Ext.isEmpty(records) || Ext.isEmpty(rawValue)) {
                return;
            }

            rawValue = rawValue.getValues();
            records = records.filter(function (item) {
                return Ext.Array.contains(rawValue, item.get(this.valueField));
            }, this);

            if (this.multiSelect) {
                selModel.checkRows(records);
                this.updateButtonsState();
            } else {
                selModel.select(records);
            }
        }
    },

    updateButtonsState: function () {
        if (this.picker) {
            var selModel = this.getGrid().getSelectionModel(),
                isEnabled = (!this.multiSelect && selModel.hasSelection()) || (this.multiSelect && selModel.hasChecked());

            this.picker.down('#select')[isEnabled ? 'enable' : 'disable']();
        }
    },

    getGrid: function () {
        return this.picker.down(this.selectorWidget).down('grid');
    }
    */
});