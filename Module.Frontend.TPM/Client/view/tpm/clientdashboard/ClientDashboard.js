Ext.define('App.view.tpm.clientdashboard.ClientDashboard', {
    extend: 'App.view.core.common.CombinedDirectoryPanel',
    alias: 'widget.clientdashboard',
    title: l10n.ns('tpm', 'mainmenu').value('Dashboard'),

    dockedItems: [],
    customHeaderItems: [],
    getBaseModel: function (exactryModel) { },
    getDefaultResource: function (exactryModel) { },

    items: [{
        xtype: 'container',
        itemId: 'clientDashboardFirstChildContainer',
        cls: 'client-dashboard-first-child-container',
        layout: 'vbox',
        defaults: {
            width: '100%'
        },
        items: [{
            xtype: 'fieldset',
            cls: 'client-dashboard-fieldset',
            items: [{
                xtype: 'panel',
                dockedItems: [{
                    xtype: 'toolbar',
                    dock: 'top',
                    cls: 'client-dashboard-toolbar',
                    items: [
                        {
                            xtype: 'button',
                            glyph: 0xF420,
                            text: 'Account Information',
                            itemId: 'accountInformationButton',
                            cls: 'approval-status-history-btn selected client-dashboard-toolbar-button client-dashboard-toolbar-button-selected',
                            active: true,
                            listeners: {
                                beforerender: function (button) {
                                    var clientDashboardController = App.app.getController('tpm.clientdashboard.ClientDashboard');
                                    button.addListener('click', clientDashboardController.onAccountInformationButtonClick);
                                }
                            }
                        },
                        {
                            xtype: 'button',
                            glyph: 0xF35C,
                            text: 'Promo week & VoD',
                            itemId: 'promoWeeksButton',
                            cls: 'approval-status-history-btn client-dashboard-toolbar-button',
                            active: false,
                            listeners: {
                                beforerender: function (button) {
                                    var clientDashboardController = App.app.getController('tpm.clientdashboard.ClientDashboard');
                                    button.addListener('click', clientDashboardController.onPromoWeeksButtonClick);
                                }
                            }
                        },
                        {
                            xtype: 'button',
                            glyph: 0xF420,
                            text: 'Account Information RS',
                            itemId: 'accountInformationRSButton',
                            cls: 'approval-status-history-btn client-dashboard-toolbar-button',
                            active: false,
                            listeners: {
                                beforerender: function (button) {
                                    var clientDashboardController = App.app.getController('tpm.clientdashboard.ClientDashboard');
                                    button.addListener('click', clientDashboardController.onAccountInformationRSButtonClick);
                                }
                            }
                        },
                        {
                            xtype: 'button',
                            glyph: 0xF35C,
                            text: 'Promo week & VoD RS',
                            itemId: 'promoWeeksRSButton',
                            cls: 'approval-status-history-btn client-dashboard-toolbar-button',
                            active: false,
                            listeners: {
                                beforerender: function (button) {
                                    var clientDashboardController = App.app.getController('tpm.clientdashboard.ClientDashboard');
                                    button.addListener('click', clientDashboardController.onPromoWeeksRSButtonClick);
                                }
                            }
                        },
                    ]
                }]
            }]
        },
        {
            xtype: 'container',
            flex: 1,
            cls: 'client-dashboard-selected-container',
            itemId: 'selectedContainer',
            layout: 'vbox',
            defaults: {
                flex: 1,
                width: '100%'
            },
            items: [
                {
                    xtype: 'accountinformation'
                },
                {
                    xtype: 'promoweeks',
                    hidden: true
                },
                {
                    xtype: 'accountinformationrs',
                    hidden: true
                },
                {
                    xtype: 'promoweeksrs',
                    hidden: true
                },
            ]
        }
        ]
    }]
});