Ext.define('App.view.core.common.FilterableSearchComboBox', {
    extend: 'Ext.form.field.ComboBox',
    alias: 'widget.fsearchcombobox',

    trigger1Cls: Ext.baseCSSPrefix + 'form-arrow-trigger',
    trigger2Cls: Ext.baseCSSPrefix + 'form-search-trigger',
    trigger3Cls: Ext.baseCSSPrefix + 'form-clear-trigger',

    multiSelect: false,
    editable: false,
    autoSelect: false,
    forceSelection: true,

    delimiter: ';',

    // Предполагается, что виджет, указанный в selectorWidget, 
    // является наслендиком Ext.grid.Panel.
    selectorWidget: undefined,

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
        var window = this.createWindow(),
            grid, columns;

        if (window) {
            window.show();
            grid = this.getGrid();

            if (this.multiSelect) {
                columns = grid.headerCt.getGridColumns();
                if (columns.length > 0 && columns[0].hasOwnProperty('isCheckerHd')) {
                    columns[0].show();
                }
            }

            grid.getStore().load();
        }
    },

    onTrigger3Click: function () {
        this.clearValue();
    },

    onSelectionChange: function (selModel) {
        this.updateButtonsState();
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
        this.fireEvent('select', this, value);
        this.window.close();
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

        window = this.window = Ext.widget('selectorwindow', {
            title: l10n.ns('core').value('selectorWindowTitle'),
            items: [{
                xtype: this.selectorWidget,
                isSearch: true
            }]
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

        if (value && Ext.getClassName(value) !== 'App.extfilter.core.ValueSearchList') {
            value = Ext.create('App.extfilter.core.ValueSearchList', value, this.valueField, this.delimiter);
        }

        var records = value ? value.getRecords() : [];
        this.displayTplData = records ? Ext.Array.pluck(records, 'data') : records;
        this.lastSelection = this.valueModels = records;
        this.value = value;
        this.setRawValue(this.valueToRaw(value));
        this.checkChange();

        if (doSelect !== false) {
            this.syncSelection();
        }

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
        return value ? value.toStringByField(this.displayField, this.itemRenderer, this) : '';
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
        if (this.window) {
            var selModel = this.getGrid().getSelectionModel(),
                isEnabled = (!this.multiSelect && selModel.hasSelection()) || (this.multiSelect && selModel.hasChecked());

            this.window.down('#select')[isEnabled ? 'enable' : 'disable']();
        }
    },

    getGrid: function () {
        return this.window.down(this.selectorWidget).down('grid');
    },

    getSubmitValue: function () {
        if (!this.value) return;
        var values = this.value.getValues();
        return values && values.length == 1 ? values[0] : values;
    }
});