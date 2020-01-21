Ext.define('App.view.tpm.promo.PromoEvent', {
    extend: 'Ext.panel.Panel',
    alias: 'widget.promoevent',

    eventId: null,
    EventName: null,

    items: [{
        xtype: 'container',
        cls: 'custom-promo-panel-container',
        layout: {
            type: 'hbox',
            align: 'stretchmax'
        },
        items: [{
            xtype: 'custompromopanel',
            name: 'chooseEvent',
            minWidth: 245,
            flex: 1,
            items: [{
                xtype: 'fieldset',
                title: 'Choose event',
                padding: '1 4 0 4',
                items: [{
                    xtype: 'form',
                    layout: {
                        type: 'hbox',
                        align: 'stretch'
                    },
                    items: [{
                        xtype: 'container',
                        padding: '0 0 0 5',
                        items: [{
                            xtype: 'chooseEventButton',
                            name: 'EventId',
                            selectorWidget: 'event',
                            valueField: 'Id',
                            glyph: 0xf9d2,
                            scale: 'large',
                            height: 98,
                            width: 52,
                            text: '<b>...</b>',
                            iconAlign: 'top',
                            enableToggle: true,
                            allowDepress: true,
                            needUpdateMappings: true,
                            cls: 'custom-event-button',
                            crudAccess: ['Administrator', 'SupportAdministrator', 'FunctionalExpert', 'CMManager', 'CustomerMarketing', 'KeyAccountManager'],
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
                                to: 'PromoEventName'
                            }, {
                                from: 'Description',
                                to: 'PromoEventDescription'
                            }]
                        }]
                    }, {
                        xtype: 'container',
                        flex: 1,
                        padding: '0 5 0 7',
                        items: [{
                            xtype: 'singlelinedisplayfield',
                            name: 'PromoEventName',
                            width: '100%',
                            fieldLabel: l10n.ns('tpm', 'Promo').value('EventName'),
                            listeners: {
                                change: function (field, newValue, oldValue) {
                                    if (newValue) {
                                        var promoEventButton = Ext.ComponentQuery.query('button[itemId=btn_promo_step5]')[0];
                                        promoEventButton.setText('<b>' + l10n.ns('tpm', 'promoStap').value('basicStep5') + '</b><br><p> ' + newValue + '</p>');
                                        promoEventButton.removeCls('notcompleted');
                                        promoEventButton.setGlyph(0xf133);
                                        promoEventButton.isComplete = true;
                                    }
                                }
                            }
                        }, {
                            xtype: 'hiddenfield',
                            name: 'PromoEventDescription',
                            listeners: {
                                change: function (field, newValue, oldValue) {
                                    field.up('promoevent').down('label[itemId=promoEventDescription]').setText(newValue);
                                }
                            }
                        }]
                    }]
                }]
            }]
        }, {
            xtype: 'splitter',
            itemId: 'splitter_5',
            cls: 'custom-promo-panel-splitter',
            collapseOnDblClick: false,
            listeners: {
                dblclick: {
                    fn: function (event, el) {
                        var cmp = Ext.ComponentQuery.query('splitter[itemId=splitter_5]')[0];
                        cmp.tracker.getPrevCmp().flex = 1;
                        cmp.tracker.getNextCmp().flex = 1;
                        cmp.ownerCt.updateLayout();
                    },
                    element: 'el'
                }
            }
        }, {
            xtype: 'custompromopanel',
            itemId: 'promoEventDescription',
            minWidth: 245,
            flex: 1,
            items: [{
                xtype: 'fieldset',
                title: 'Description',
                items: [{
                    xtype: 'label',
                    itemId: 'promoEventDescription',
                    margin: '0 0 0 4'
                }]
            }]
        }]
    }]
})