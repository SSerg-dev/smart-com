Ext.define('App.view.tpm.btl.BTLEditor', {
    extend: 'App.view.core.common.EditorDetailWindow',
    alias: 'widget.btleditor',
    cls: 'readOnlyFields',

    items: {
        xtype: 'editorform',
        columnsCount: 1,
        items: [{
            xtype: 'numberfield',
            name: 'PlanBTLTotal',
            labelWidth: '105px',
            fieldLabel: l10n.ns('tpm', 'BTL').value('PlanBTLTotal'),
            minValue: 0,
        }, {
            xtype: 'numberfield',
            name: 'ActualBTLTotal',
            labelWidth: '105px',
            fieldLabel: l10n.ns('tpm', 'BTL').value('ActualBTLTotal'),
            minValue: 0,
        },  {
            xtype: 'container',
            flex: 1,
            layout: {
                type: 'hbox',
                align: 'stretch'
            },
            defaults: {
                labelAlign: 'left',
                flex: 1,
            },
            items: [{
                xtype: 'datefield',
                cls: 'x-field x-table-plain x-form-item x-form-type-text x-form-dirty x-box-item x-field-detail-form-field x-vbox-form-item',
                name: 'StartDate',
                fieldLabel: l10n.ns('tpm', 'BTL').value('StartDate'),
                allowBlank: false,
                editable: false,
                format: 'd.m.Y',
                listeners: {
                    change: function (field) {
                        var endDate = field.up('editorform').down('[name=EndDate]');

                        if (field.getValue()) {
                            var minValue = new Date(Date.parse(field.getValue()) + 24 * 60 * 60 * 1000);
                            endDate.setMinValue(minValue);
                        }
                    }
                }
            }, {
                xtype: 'container',
                flex: 0.1,
                layout: {
                    type: 'hbox',
                    align: 'top',
                    pack: 'center',
                    width: 1,
                },
            }, {
                xtype: 'datefield',
                name: 'EndDate',
                cls: 'x-field x-table-plain x-form-item x-form-type-text x-form-dirty x-box-item x-field-detail-form-field x-vbox-form-item',
                fieldLabel: l10n.ns('tpm', 'BTL').value('EndDate'),
                allowBlank: false,
                editable: false,
                format: 'd.m.Y',
                listeners: {
                    change: function (field) {
                        var startDate = field.up('editorform').down('[name=StartDate]');

                        if (field.getValue()) {
                            var maxValue = new Date(Date.parse(field.getValue()) - 24 * 60 * 60 * 1000);
                            startDate.setMaxValue(maxValue);
                        }
                    },
                }
            }]
        },  {
            xtype: 'searchfield',
            fieldLabel: l10n.ns('tpm', 'Event').value('Name'),
            name: 'EventId',
            labelWidth: '105px',
            selectorWidget: 'event',
            valueField: 'Id',
            displayField: 'Name',
            store: {
                type: 'directorystore',
                model: 'App.model.tpm.event.Event',
                extendedFilter: {
                    xclass: 'App.ExtFilterContext',
                    supportedModels: [{
                        xclass: 'App.ExtSelectionFilterModel',
                        model: 'App.model.tpm.event.Event',
                        modelId: 'efselectionmodel'
                    }]
                }
            },
            mapping: [{
                from: 'Name',
                to: 'EventName'
            }]
        },  {
            xtype: 'textfield',
            name: 'InvoiceNumber',
            labelWidth: '105px',
            allowBlank: true,
            allowOnlyWhitespace: true,
            regex: /^[0-9]*[0-9]$/,
            fieldLabel: l10n.ns('tpm', 'BTL').value('InvoiceNumber'),
            regexText: l10n.ns('tpm', 'BTL').value('InvoiceNumberRegex'),
        }]
    }
});

