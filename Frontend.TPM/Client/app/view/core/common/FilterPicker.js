Ext.define('App.view.core.common.FilterPicker', {
    extend: 'App.view.core.base.BaseModalWindow',
    alias: 'widget.filterpicker',

    width: '50%',
    minHeight: 200,

    layout: {
        type: 'vbox',
        align: 'stretch'
    },

    items: [{
        xtype: 'filterform',
        flex: 1,
        dockedItems: [{
            xtype: 'panel',
            ui: 'detailform-panel',
            dock: 'top',
            height: 10,

            layout: {
                type: 'hbox',
                pack: 'center',
                align: 'middle'
            },

            items: [{
                xtype: 'label',
                itemId: 'formtitle',
                // text: l10n.ns('core').value('editingModeTitle'),
                cls: 'x-form-item-label',
                hidden: true
            }]
        }]
    }],
    
    buttons: [{
        text: l10n.ns('core', 'buttons').value('cancel'),
        action: 'cancel'
    }, {
        text: l10n.ns('core', 'buttons').value('ok'),
        ui: 'green-button-footer-toolbar',
        action: 'ok'
    }],

    constructor: function (cfg) {
        this.items[0].items = this.buildItems(cfg.viewItems);

        this.callParent(arguments);
        this.addEvents('select');
    },

    buildItems: function(sourceItems) {
        var result = [];
        for (var i = 0; i < sourceItems.length; i++) {
            var item = sourceItems[i];
            result.push(this.editorFactory(item));
        }
        return result;
    },

    editorFactory: function (cfgItem) {
        if (cfgItem.operation === 'Equals') {
            return {
                xtype: 'searchfield',
                fieldLabel: cfgItem.displayName,
                name: cfgItem.fieldName,
                fieldType: 'filterField',
                selectorWidget: cfgItem.selectorWidget,
                operation: cfgItem.operation,
                valueField: 'Id',
                displayField: cfgItem.displayField,
                store: {
                    type: 'directorystore',
                    model: cfgItem.model,
                    extendedFilter: {
                        xclass: 'App.ExtFilterContext',
                        supportedModels: [{
                            xclass: 'App.ExtSelectionFilterModel',
                            model: cfgItem.model,
                            modelId: 'efselectionmodel'
                        }]
                    }
                }
            };
        } else if (cfgItem.operation === 'List') {
            return {
                xtype: 'fsearchfield',
                multiSelect: true,
                fieldLabel: cfgItem.displayName,
                name: cfgItem.fieldName,
                fieldType: 'filterField',
                selectorWidget: cfgItem.selectorWidget,
                operation: cfgItem.operation,
                valueField: 'Id',
                displayField: cfgItem.displayField,
                setValue: function (value) {
                    var processedValue = [];
                    var displayValue = '';
                    if (value) {
                        var values = value && value.value ? value.value : value;
                        processedValue = Ext.Array.map(values, function (item) {
                            return item.get ? item.get(this.valueField) : item;
                        }, this);
                        displayValue = value.display || Ext.Array.map(values, function (item) {
                            return item.get(this.displayField);
                        }, this).join(this.delimiter);
                    }
                    this.value = processedValue;
                    this.setRawValue(displayValue);
                    this.checkChange();
                    return this;
                },
                getValue: function () {
                    return this.value;
                },
                onFirstLoad: function (store) {
                    var selModel = this.getGrid().getSelectionModel();
                    if (this.value) {
                        var displayValues = (this.getRawValue() || '').split(this.delimiter);
                        var records = Ext.Array.map(this.value, function (item, idx) {
                            var rec = Ext.create(cfgItem.model);
                            rec.set(this.valueField, item);
                            rec.set(this.displayField, displayValues[idx]);
                            return rec;
                        }, this);
                        if (Ext.isEmpty(records)) {
                            return;
                        }
                        if (this.multiSelect) {
                            selModel.checkRows(records);
                            this.updateButtonsState();
                        } else {
                            selModel.select(records);
                        }
                    }
                },
                store: {
                    type: 'directorystore',
                    model: cfgItem.model,
                    extendedFilter: {
                        xclass: 'App.ExtFilterContext',
                        supportedModels: [{
                            xclass: 'App.ExtSelectionFilterModel',
                            model: cfgItem.model,
                            modelId: 'efselectionmodel'
                        }]
                    }
                }
            };
        } else {
            throw Ext.String.format('Operation "{0}" not supported', cfgItem.operation);
        }
    },

    initComponent: function () {
        this.callParent(arguments);

        this.cancelButton = this.down('button[action=cancel]');
        this.selectButton = this.down('button[action=ok]');
    },

    initEvents: function () {
        this.callParent(arguments);

        this.cancelButton.on('click', this.onCancelButtonClick, this);
        this.selectButton.on('click', this.onSelectButtonClick, this);
    },

    onCancelButtonClick: function (btn) {
        this.close();
    },

    onSelectButtonClick: function (btn) {
        this.updateData();
        this.fireEvent('select', this, this.getValue());
    },

    afterRender: function () {
        this.callParent(arguments);
        this.updateView();
    },

    setValue: function (value) {
        this.value = value;
        this.updateView();
    },

    getValue: function () {
        return this.value;
    },

    updateView: function () {
        var value = this.getValue();
        var filter = value ? value.filter || [] : [];
        var fields = Ext.ComponentQuery.query('component[fieldType=filterField]', this);
        Ext.Array.forEach(fields, function (field) {
            if (field.operation === 'Equals') {
                var filterItem = Ext.Array.findBy(filter, function (it) { return it.fieldName === field.name; });
                var val = {
                    value: null,
                    display: null
                };
                if (filterItem) {
                    val.value = filterItem.value ? filterItem.value[0] : null;
                    val.display = filterItem.displayValue;
                }
                field.setValue(val);
            } else if (field.operation === 'List') {
                var filterItem = Ext.Array.findBy(filter, function (it) { return it.fieldName === field.name; });
                var val = null;
                if (filterItem) {
                    val = {
                        value: filterItem.value,
                        display: filterItem.displayValue
                    };
                }
                field.setValue(val);
            }
        });
    },

    updateData: function () {
        var newValue = {
            displayFilter: '',
            filter: []
        };
        var fields = Ext.ComponentQuery.query('component[fieldType=filterField]', this);
        Ext.Array.forEach(fields, function (item) {
            var val = item.getValue();
            if (Ext.isString(val)) {
                val = [val];
            }
            if (val && val.length) {
                newValue.filter.push({
                    fieldName: item.name,
                    value: val,
                    displayValue: item.getRawValue()
                });
            }
        });
        newValue.displayFilter = this.getDisplayFilter(newValue);
        this.value = newValue;
    },

    getDisplayFilter: function (value) {
        if (value && value.filter && value.filter.length > 0) {
            if (value.filter.length > 1) {
                return l10n.ns('core').value('BigFilterDisplay');
            } else {
                var item = value.filter[0];
                return Ext.String.format('{0}: {1}', item.fieldName, item.displayValue);
            }
        } else {
            return '';
        }
    }

});
