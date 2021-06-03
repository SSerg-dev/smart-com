Ext.define('App.view.tpm.promo.ApproveClosePromoWindow', {
    extend: 'App.view.core.base.BaseModalWindow',
    alias: 'widget.approveclosepromowindow',
    title: l10n.ns('tpm', 'compositePanelTitles').value('ApproveClosePromoWindow'),
    cls: 'promo-approve-close-window',

    width: 675,
    height: 455,
    resizable: false,

    items: [{
        xtype: 'container',
        height: '100%',
        maxHeight: '100%',
        cls: 'custom-promo-panel-container',
        padding: '5 5 5 5',
        items: [{
            xtype: 'container',
            cls: 'approve-close-promo-window-container',
            layout: 'auto',
            items: [{
                xtype: 'fieldset',
                height: '140px',
                margin: '5 10 5 10',
                title: l10n.ns('tpm', 'ApproveClosePromoWindow').value('Parameters'),
                layout: {
                    type: 'vbox',
                    align: 'stretch'
                },
                items: [{
                    xtype: 'textfield',
                    name: 'PromoID',
                    fieldLabel: l10n.ns('tpm', 'ApproveClosePromoWindow').value('PromoID'),
                    flex: 1,
                    layout: 'anchor',
                    readOnlyCls: 'readOnlyField',
                    labelAlign: 'left',
                    readOnly: true
                }, {
                    xtype: 'textfield',
                    name: 'PromoDuration',
                    fieldLabel: l10n.ns('tpm', 'ApproveClosePromoWindow').value('PromoDuration'),
                    flex: 1,
                    layout: 'anchor',
                    readOnlyCls: 'readOnlyField',
                    labelAlign: 'left',
                    readOnly: true
                }, {
                    xtype: 'textfield',
                    name: 'ShopperTI',
                    fieldLabel: l10n.ns('tpm', 'ApproveClosePromoWindow').value('ShopperTI'),
                    flex: 1,
                    layout: 'anchor',
                    readOnlyCls: 'readOnlyField',
                    labelAlign: 'left',
                    readOnly: true
                }]
            }]
        }, {
            xtype: 'container',
            cls: 'approve-close-promo-window-container',
            layout: 'auto',
            items: [{//7569
                xtype: 'gridpanel',
                cls: 'default-gridpanel',
                id: 'ApproveClosePromoWindowGrid',
                title: l10n.ns('tpm', 'ApproveClosePromoWindow').value('MarketingLinked'),
                store: {
                    type: 'directorystore',
                    model: 'App.model.tpm.promosupportpromo.PromoSupportPromoWithPromoId',
                    storeId: 'approveclosepromowindowstore',
                    autoLoad: false,
                    extendedFilter: {
                        xclass: 'App.ExtFilterContext',
                        supportedModels: [{
                            xclass: 'App.ExtSelectionFilterModel',
                            model: 'App.model.tpm.promosupportpromo.PromoSupportPromoWithPromoId',
                            modelId: 'efselectionmodel'
                        }, {
                            xclass: 'App.ExtTextFilterModel',
                            modelId: 'eftextmodel'
                        }]
                    },
                    sorters: [{
                        property: 'Number',
                        direction: 'DESC'
                    }],
                },
                columns: [
                    { text: 'Number', dataIndex: 'SupportNumber', flex: 1, text: l10n.ns('tpm', 'PromoSupportPromoWithPromoId').value('Number') },
                    { text: 'Support Type', dataIndex: 'BudgetItemName', flex: 1 },
                    { text: 'Start Date', dataIndex: 'StartDate', xtype: 'datecolumn', flex: 1 },
                    { text: 'End Date', dataIndex: 'EndDate', xtype: 'datecolumn', flex: 1 }
                ],

                viewConfig: {
                    getRowClass: function (record, rowIndex, rowParams, store) {
                        var suppStartDate = record.get('StartDate');
                        var suppEndDate = record.get('EndDate');
                        var window = Ext.ComponentQuery.query('#ApproveClosePromoWindowGrid')[0];
                        var promoStartDate = window.PromoStartDate;
                        var promoEndDate = window.PromoEndDate;

                        if (suppStartDate <= promoStartDate && suppEndDate >= promoEndDate) {
                            style = ''
                        } else {
                            style = 'bad-support'
                        }
                        return style;
                    },
                    stripeRows: false
                }
            }]
        }]
    }],

    buttons: [{
        xtype: 'tbspacer',
        flex: 10
    }, {
        text: l10n.ns('tpm', 'customtoptoolbar').value('cancel'),
        action: 'cancel',
        handler: function () {
            this.up('window').close();
        }
    }, {
        text: l10n.ns('tpm', 'customtoptoolbar').value('approve'),
        itemId: 'approveClosePromoButton',
        style: { "background-color": "#66BB6A" },
    }]
});
