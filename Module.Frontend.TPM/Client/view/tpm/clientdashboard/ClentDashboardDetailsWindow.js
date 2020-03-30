Ext.define('App.view.tpm.promo.ClentDashboardDetailsWindow', {
    extend: 'App.view.core.base.BaseModalWindow',
    alias: 'widget.clentdashboarddetailswindow',
    title: l10n.ns('tpm', 'compositePanelTitles').value('ClientDetailsWindow'),
    cls: 'promo-finance-details-window',

    width: "70%",
    minWidth: 800,
    minHeight: 265,
    items: [{
        xtype: 'container',
        height: '100%',
        maxHeight: '100%',
        cls: 'custom-promo-panel-container',
        layout: 'fit',
        items: [{
            xtype: 'custompromopanel',
            cls: 'custom-promo-panel scrollpanel',
            layout: {
                type: 'hbox',
                align: 'stretchmax',
            },
            items: [{
                xtype: 'container',
                margin: '0 10 0 0',
                flex: 1,
                layout: {
                    type: 'vbox',
                    align: 'stretch'
                },
                items: [{
                    xtype: 'fieldset',
                    title: l10n.ns('tpm', 'ClientDashboardDetailsWindow').value('MarketingYTD'),
                    padding: '0 10 10 10',
                    defaults: {
                        cls: 'borderedField-with-lable promo-finance-details-window-field',
                        labelCls: 'borderedField-label',
                        labelWidth: 140,
                        labelSeparator: '',
                        readOnly: true,
                        flex: 1,
                        margin: '0 10 0 0',
                        //editable: false,
                    },
                    layout: {
                        type: 'vbox',
                        align: 'stretch'
                    },
                    items: [
                        {
                            xtype: 'numberfield',
                            layout: 'anchor',
                            name: 'XSitesYTD',
                            fieldLabel: l10n.ns('tpm', 'ClientDashboardDetailsWindow').value('XSitesYTD'),
                            labelAlign: 'left', 
                            labelCls: 'borderedField-label',
                            width: '100%',
                            labelWidth: 190,
                            labelSeparator: '',
                            margin: '5 0 0 0',
                        }, {
                            xtype: 'numberfield',
                            layout: 'anchor',
                            name: 'CatalogueYTD',
                            fieldLabel: l10n.ns('tpm', 'ClientDashboardDetailsWindow').value('CatalogueYTD'),
                            labelAlign: 'left', 
                            labelCls: 'borderedField-label',
                            width: '100%',
                            labelWidth: 190,
                            labelSeparator: '',
                            margin: '5 0 0 0',
                        }, {
                            xtype: 'numberfield',
                            layout: 'anchor',
                            name: 'POSMInClientYTD',
                            fieldLabel: l10n.ns('tpm', 'ClientDashboardDetailsWindow').value('POSMInClientYTD'),
                            labelAlign: 'left', 
                            labelCls: 'borderedField-label',
                            width: '100%',
                            labelWidth: 190,
                            labelSeparator: '',
                            margin: '5 0 0 0',
                        }]
                }]
            }, {
                xtype: 'container',
                flex: 1,
                    items: [{
                        xtype: 'fieldset',
                        title: l10n.ns('tpm', 'ClientDashboardDetailsWindow').value('MarketingYEE'),
                        padding: '0 10 10 10',
                        defaults: {
                            cls: 'borderedField-with-lable promo-finance-details-window-field',
                            labelCls: 'borderedField-label',
                            labelWidth: 140,
                            labelSeparator: '',
                            readOnly: true,
                            flex: 1,
                            margin: '0 10 0 0',
                            //editable: false,
                        },
                        layout: {
                            type: 'vbox',
                            align: 'stretch'
                        },
                        items: [
                            {
                                xtype: 'numberfield',
                                layout: 'anchor',
                                name: 'XSitesYEE',
                                fieldLabel: l10n.ns('tpm', 'ClientDashboardDetailsWindow').value('XSitesYEE'),
                                labelAlign: 'left', 
                                labelCls: 'borderedField-label',
                                width: '100%',
                                labelWidth: 190,
                                labelSeparator: '',
                                margin: '5 0 0 0',
                            }, {
                                xtype: 'numberfield',
                                layout: 'anchor',
                                name: 'CatalogueYEE',
                                fieldLabel: l10n.ns('tpm', 'ClientDashboardDetailsWindow').value('CatalogueYEE'),
                                labelAlign: 'left', 
                                labelCls: 'borderedField-label',
                                width: '100%',
                                labelWidth: 190,
                                labelSeparator: '',
                                margin: '5 0 0 0',
                            }, {
                                xtype: 'numberfield',
                                layout: 'anchor',
                                name: 'POSMInClientTiYEE',
                                fieldLabel: l10n.ns('tpm', 'ClientDashboardDetailsWindow').value('POSMInClientTiYEE'),
                                labelAlign: 'left', 
                                labelCls: 'borderedField-label',
                                width: '100%',
                                labelWidth: 190,
                                labelSeparator: '',
                                margin: '5 0 0 0',
                            }]
                    }]
            }]
        }]
    }],


    buttons: [{
        text: l10n.ns('core', 'buttons').value('cancel'),
        itemId: 'close'
    }]
});