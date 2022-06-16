Ext.define('App.view.tpm.event.EventEditor', {
    extend: 'App.view.core.common.EditorDetailWindow',
    alias: 'widget.eventeditor',
    width: 500,
    minWidth: 500,
    maxHeight: 500,
    cls: 'readOnlyFields',

    afterWindowShow: function (scope, isCreating) {
        scope.down('field').focus(true, 10); // фокус на первом поле формы для корректной работы клавишных комбинаций

        if (scope.down('textfield[name=Name]').rawValue == 'Standard promo') {
            scope.down('textfield[name=Name]').setReadOnly(true);
            scope.down('textfield[name=Description]').setReadOnly(true);
        }
    },

    items: {
        xtype: 'editorform',
        columnsCount: 1,

        items: [
            {
                xtype: 'textfield',
                name: 'Name',
                fieldLabel: l10n.ns('tpm', 'Event').value('Name')
            },
            {
                xtype: 'textfield',
                name: 'Description',
                allowBlank: true,
                allowOnlyWhitespace: true,
                fieldLabel: l10n.ns('tpm', 'Event').value('Description')
            },
            {
                xtype: 'searchcombobox',
                fieldLabel: l10n.ns('tpm', 'Event').value('EventTypeName'),
                name: 'EventTypeId',
                selectorWidget: 'eventtype',
                valueField: 'Id',
                displayField: 'Name',
                store: {
                    type: 'simplestore',
                    model: 'App.model.tpm.eventtype.EventType',
                    extendedFilter: {
                        xclass: 'App.ExtFilterContext',
                        supportedModels: [{
                            xclass: 'App.ExtSelectionFilterModel',
                            model: 'App.model.tpm.eventtype.EventType',
                            modelId: 'efselectionmodel'
                        }]
                    },
                    //listeners: {
                    //    load: function (store) {
                    //        debugger;
                    //        var EventTypeId = Ext.ComponentQuery.query('searchcombobox[name=EventTypeId]')[0];
                    //        var record = store.getById(EventTypeId.value);
                    //        if (!record.data.National) {
                    //            var segmentSet = Ext.ComponentQuery.query('fieldset[itemId=segmentSet]')[0];
                    //            segmentSet.setDisabled(true);
                    //        }
                    //    },
                    //}
                },
                mapping: [{
                    from: 'Name',
                    to: 'EventTypeName'
                }],
                listeners: {
                    change: function (field, newValue, oldValue) {
                        if (newValue) {
                            var segmentSet = Ext.ComponentQuery.query('fieldset[itemId=segmentSet]')[0];
                            if (field.record.data.National) {
                                segmentSet.setDisabled(false);
                            }
                            else {
                                segmentSet.setDisabled(true);
                                var marketSegmentCombobox = Ext.ComponentQuery.query('combobox[name=MarketSegment]')[0];
                                debugger;
                                marketSegmentCombobox.setValue('');
                            }
                        }
                    }
                }
            },
            {
                xtype: 'fieldset',
                title: 'Segment',
                itemId: 'segmentSet',
                padding: '0 5 5 7',
                layout: {
                    type: 'hbox',
                    align: 'stretchmax',
                },
                items: [
                    {
                        xtype: 'combobox',
                        flex: 5,
                        name: 'MarketSegment',
                        labelWidth: 160,
                        valueField: 'segment',
                        displayField: 'segment',
                        queryMode: 'local',
                        editable: false,
                        fieldLabel: l10n.ns('tpm', 'Event').value('MarketSegment'),
                        store: Ext.create('Ext.data.Store', {
                            fields: ['segment'],
                            data: [
                                { segment: 'Catcare' },
                                { segment: 'Dogcare' },
                            ]
                        }),
                        mapping: [{
                            from: 'segment',
                            to: 'MarketSegment'
                        }],
                    },
                    {
                        xtype: 'container',
                        margin: '0 5 0 5'
                    },
                    {
                        xtype: 'checkbox',
                        flex: 1,
                        labelSeparator: '',
                        itemId: 'segmentCheckbox',
                        boxLabel: 'All',
                        //labelAlign: 'right',
                        //style: 'margin-left: 10px',
                        listeners: {
                            change: function (checkbox, newValue, oldValue) {
                                var marketSegmentCombobox = Ext.ComponentQuery.query('combobox[name=MarketSegment]')[0];
                                if (newValue) {
                                    marketSegmentCombobox.setValue('');
                                }
                                marketSegmentCombobox.setDisabled(newValue);
                            },
                        }
                    }
                ]
            }
        ]
    }
});




