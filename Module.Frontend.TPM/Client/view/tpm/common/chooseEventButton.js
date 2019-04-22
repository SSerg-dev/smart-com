Ext.define('App.view.tpm.common.chooseEventButton', {
    //extend: 'Ext.form.field.Trigger',
    extend: 'Ext.button.Button',
    alias: 'widget.chooseEventButton',
    mixins: ['Ext.util.Bindable'],

    // Предполагается, что виджет, указанный в selectorWidget, 
    // является наслендиком Ext.grid.Panel.
    selectorWidget: undefined,
    editable: false,
    isSearchField: true,
    needUpdateMappings: false,
    defaultStoreState: null,
    selectValidate: true,

    validator: function () {
        if (!this.selectValidate) {
            return l10n.ns('core', 'customValidators').value('invalidSearchFieldValue')
        }
        return true;
    },

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

        if (picker) {
            picker.down(this.selectorWidget).down('grid').bindStore(store, initial);
        }
    },

    listeners: {
        click: function () {
            var picker = this.createPicker();

            if (picker) {
                picker.show();
                this.getStore().load();
            }
        },
    },

    onSelectionChange: function (selModel) {
        var picker = this.picker,
            hasSelection = selModel.hasSelection();

        if (picker) {
            picker.down('#select')[hasSelection ? 'enable' : 'disable']();
        }
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
        var picker = this.picker,
            selModel = picker.down(this.selectorWidget).down('grid').getSelectionModel(),
            record = selModel.hasSelection() ? selModel.getSelection()[0] : null;

        this.setValue(record);
        if (this.needUpdateMappings) {
            this.updateMappingValues(record);
        }
        this.afterSetValue(record);
        picker.close();
    },

    updateMappingValues: function (record) {
        var el = this.up('form');
        if (el && this.mapping) {
            var form = el.getForm();
            for (var i in this.mapping) {
                var map = this.mapping[i];
                var val = record.get(map.from);
                var field = form.findField(map.to);
                if (field) {
                    field.setValue(val);
                }
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
            items: [selectorWidgetConfig]
        });

        var grid = picker.down(this.selectorWidget).down('grid');

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

    addHandlers: function (picker) {
        var me = this;

        picker.down(this.selectorWidget).down('grid').on('selectionchange', this.onSelectionChange, this);
        picker.down(this.selectorWidget).down('grid').on('itemdblclick', this.onSelectiondbClick, this);
        picker.down('#select').on('click', this.onSelectButtonClick, this);
        picker.down('#cancel').on('click', this.onCancelButtonClick, this);

        picker.on('close', function (panel, e) {
            var searchfieldstore = me.getStore(),
                value = me.getValue();

            if (value) {
                var num = searchfieldstore.find('Id', value);
                if (num === -1) {
                    me.selectValidate = false;
                    //me.validate();
                }
                else {
                    me.selectValidate = true;
                    //me.validate();
                }
            }
        });
    },

    clearValue: function () {
        this.setValue(null);
    },

    setValue: function (value) {
        var record = value;
        var store = this.getStore();
        var processedValue;
        var displayValue;
        var idx;

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
        //this.setRawValue(displayValue);
        this.record = record;
        //this.checkChange();

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