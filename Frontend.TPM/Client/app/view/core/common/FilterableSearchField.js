Ext.define('App.view.core.common.FilterableSearchField', {
    extend: 'Ext.form.field.Trigger',
    alias: 'widget.fsearchfield',
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

        if (picker) {
            this.getGrid().bindStore(store, initial);
        }
    },

    onTrigger1Click: function () {
        var picker = this.createPicker(),
            grid, columns;

        if (picker) {
            picker.show();
            if (this.multiSelect) {
                grid = this.getGrid();
                columns = grid.headerCt.getGridColumns();
                if (columns.length > 0 && columns[0].hasOwnProperty('isCheckerHd')) {
                    columns[0].show();
                }
            }
            this.getStore().load();
        }
    },

    onTrigger2Click: function () {
        this.clearValue();
    },

    onSelectionChange: function (selModel) {
        this.updateButtonsState();
    },

    onSelectiondbClick: function (grid) {
        var win = grid.up('selectorwindow');
        var columns = grid.headerCt.query('gridcolumn');
        if (columns.length > 0 && columns[0].hasOwnProperty('isCheckerHd') && !columns[0].hidden) {
            var selModel = grid.getSelectionModel();
            record = selModel.hasSelection() ? selModel.getSelection()[0] : null;
            if (record) {
                if (selModel.isChecked(record)) {
                    selModel.uncheckRows(record);
                } else {
                    selModel.checkRows(record);
                }
                if (win) {
                    win.down('#select')[selModel.hasChecked() ? 'enable' : 'disable']();
                }
            }
        } else {
            this.onSelectButtonClick();
        }
    },

    onSelectButtonClick: function (button) {
        var selModel = this.getGrid().getSelectionModel(),
            value = null;

        if (this.multiSelect) {
            value = selModel.getCheckedRows();
        } else if (selModel.hasSelection()) {
            value = selModel.getSelection()[0];
        }

        this.setValue(value);
        this.fireEvent('select', this);
        this.picker.close();
    },

    onCancelButtonClick: function (button) {
        this.picker.close();
    },

    createPicker: function () {
        var picker,
            store = this.getStore();

        picker = this.picker = Ext.widget('selectorwindow', {
            title: l10n.ns('core').value('selectorWindowTitle'),
            items: [{
                xtype: this.selectorWidget,
                isSearch: true
            }]
        });

        var grid = this.getGrid();

        if (!this.defaultStoreState) {
            var sorters = grid.getStore().sorters;
            if ((!store.sorters || store.sorters.length == 0) && sorters && sorters.length > 0) {
                store.sorters.clear();
                store.sorters.addAll(sorters.getRange());
            }
            this.defaultStoreState = store.getState() || {};
            Ext.applyIf(this.defaultStoreState, {
                sorters: [],
                filters: []
            });
        } else {
            store.applyState(this.defaultStoreState);
        }

        grid.bindStore(store);
        this.addHandlers(picker);
        this.afterPickerCreate(this);

        return picker;
    },

    afterPickerCreate: function (scope) {
    },

    addHandlers: function (picker) {
        picker.down('#select').on('click', this.onSelectButtonClick, this);
        picker.down('#cancel').on('click', this.onCancelButtonClick, this);
        this.getGrid().on('selectionchange', this.onSelectionChange, this);
        picker.down(this.selectorWidget).down('grid').on('itemdblclick', this.onSelectiondbClick, this);
        this.getStore().on({
            single: true,
            scope: this,
            load: this.onFirstLoad
        });
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
});