Ext.define('App.view.core.filter.ExtendedFilterRow', {
    extend: 'Ext.form.FieldContainer',
    alias: 'widget.extfilterrow',
    fieldLabel: '',
    labelWidth: 170,
    maxHeight: 32,
    labelSeparator: '',
    labelClsExtra: 'singleline-lable',

    fieldLabelTip: null,
    fieldValueTip: null,

    layout: {
        type: 'hbox',
        pack: 'start',
        align: 'stretch'
    },

    defaults: {
        margin: '0 10 0 0'
    },

    items: [{
        xtype: 'combobox',
        itemId: 'operation',
        editable: false,
        width: 135,
        valueField: 'id',
        queryMode: 'local',
        store: {
            fields: ['id', 'text']
        }
    }, {
        xtype: 'checkbox',
        itemId: 'relativeflag',
        boxLabel: l10n.ns('core', 'filter').value('fieldCheckboxLabel')
    }, {
        xtype: 'container',
        itemId: 'valuecontainer',
        layout: 'fit',
        margin: 0,
        flex: 1
    }],

    constructor: function (entry, editorFactory) {
        this.filterEntry = entry;
        this.editorFactory = editorFactory;
        this.callParent();

        var operationCmp = this.operationCmp = this.down('#operation'),
            relativeFlagCmp = this.relativeFlagCmp = this.down('#relativeflag'),
            valueContainerCmp = this.valueContainerCmp = this.down('#valuecontainer');

        this.itemId = this.filterEntry.get('id');
        this.setFieldLabel(this.filterEntry.get('name'));

        this.operationCmp.getStore().loadData(this.filterEntry.getAllowedOperations().map(function (op) {
            return {
                id: op,
                text: l10n.ns('core', 'filter', 'operations').value(op)
            };
        }));

        this.updateView();

        operationCmp.on('change', this.onOperationChange, this);
        relativeFlagCmp.on('change', this.onRelativeFlagChange, this);

        this.filterEntryEventHandlers = this.filterEntry.on({
            scope: this,
            destroyable: true,
            edit: this.onFilterEntryChange,
            reject: this.onFilterEntryChange
        });
    },

    onDestroy: function () {
        this.callParent(arguments);
        Ext.destroy(this.filterEntryEventHandlers);
    },

    onOperationChange: function (cmp, newValue) {
        this.filterEntry.set('operation', newValue);
        this.filterEntry.set('value', null);
    },

    onRelativeFlagChange: function (cmp, newValue) {
        this.filterEntry.set('value', newValue ? Ext.create('App.extfilter.core.Field') : null);
    },

    onFilterEntryChange: function () {
        this.updateView();
    },

    updateView: function () {
        var isAllowedRelative = this.filterEntry.isAllowedRelative(),
            isRelative = isAllowedRelative && this.filterEntry.getValueType() === 'field';

        this.setValueSilent(this.operationCmp, this.filterEntry.get('operation'));
        this.setValueSilent(this.relativeFlagCmp, isRelative);

        this.relativeFlagCmp.setDisabled(!isAllowedRelative);

        this.updateFilterValueContainer();

        this.setValueSilent(this.valueCmp, this.beforeRead(this.filterEntry.get('value')));
    },

    setValueSilent: function (cmp, value) {
        if (!cmp) {
            return;
        }

        cmp.suspendEvents();
        cmp.setValue(value);
        cmp.resumeEvents();
    },

    beforeRead: function (value) {
        if (value && Ext.getClassName(value) === 'App.extfilter.core.Field') {
            return value.name;
        }

        return value;
    },

    beforeWrite: function (value) {
        var currentValue = this.filterEntry.get('value');

        if (currentValue && Ext.getClassName(currentValue) === 'App.extfilter.core.Field') {
            if (Ext.isString(value)) {
                return Ext.create('App.extfilter.core.Field', value);
            } else if (value.isModel) {
                return Ext.create('App.extfilter.core.Field', value.get('name'));
            }

            return Ext.create('App.extfilter.core.Field', '');
        }

        if (value === 'Interface') {
            value = value.toLowerCase();
        } else if (Ext.getClassName(value) === 'App.extfilter.core.ValueSearchList') {
            if (value.getValues().indexOf('Interface') != -1) {
                var newList = [];
                value.getValues().forEach(function (item, index, array) {
                    if (item != 'Interface') {
                        newList.push(item);
                    } else {
                        newList.push(item.toLowerCase());
                    }
                });
                value.update(newList);
            }
        }

        return value;
    },

    updateFilterValueContainer: function () {
        var currentValueCmp = this.valueContainerCmp.child(),
            valueCmp = this.valueCmp = this.editorFactory.createEditor(this.filterEntry, currentValueCmp);

        if (valueCmp !== currentValueCmp) {
            Ext.suspendLayouts();
            this.valueContainerCmp.removeAll();
            this.valueContainerCmp.add(valueCmp);
            Ext.resumeLayouts(true);
        }

        return valueCmp;
    },

    commitChanges: function () {
        if (this.valueCmp) {
            this.filterEntry.set('value', this.beforeWrite(this.valueCmp.getValue()));
        }
    },

    afterFirstLayout: function () {
        var me = this;
        this.callParent(arguments);

        if (me.labelEl) {
            me.fieldLabelTip = Ext.create('Ext.tip.ToolTip', {
                target: me.labelEl,
                preventLoseFocus: true,
                trackMouse: true,
                html: me.getFieldLabel()
            });
        }
        if (me.inputEl) {
            me.fieldValueTip = Ext.create('Ext.tip.ToolTip', {
                target: me.inputEl,
                preventLoseFocus: true,
                trackMouse: true,
                listeners: {
                    beforeshow: function (tip) {
                        var value = me.getDisplayValue ? me.getDisplayValue() : me.getRawValue()
                        tip.update(value);
                    }
                }
            });
        }
    }

});