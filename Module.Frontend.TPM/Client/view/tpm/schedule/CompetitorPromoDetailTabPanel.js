Ext.define('App.view.tpm.schedule.CompetitorPromoDetailTabPanel', {
    extend: 'App.view.core.common.CombinedDirectoryPanel',
    alias: 'widget.competitorpromodetailtabpanel',
    margin: '20 7 20 20',
    title: l10n.ns('tpm', 'compositePanelTitles').value('PromoDetail'),
    minHeight: null,
    bodyCls: 'promo-detail-panel-default',
    dockedItems: [{
        xtype: 'readonlydirectorytoolbar',
        dock: 'right',
        items: [{
            xtype: 'widthexpandbutton',
            ui: 'fill-gray-button-toolbar',
            text: l10n.ns('core', 'selectablePanelButtons').value('toolbarCollapse'),
            glyph: 0xf13d,
            glyph1: 0xf13e,
            target: function () {
                return this.up('toolbar');
            }
        }, {
            glyph: 0xfba8,
            itemId: 'promoDetail',
            text: l10n.ns('tpm', 'Promo').value('ToolbarDetail'),
            tooltip: l10n.ns('tpm', 'Promo').value('ToolbarDetail')
        }, {
            itemId: 'historybutton',
            resource: 'HistoricalCompetitorPromoes',
            action: 'GetHistoricalCompetitorPromoes',
            glyph: 0xf2da,
            text: l10n.ns('core', 'crud').value('historyButtonText'),
            tooltip: l10n.ns('core', 'crud').value('historyButtonText')
        }]
    }],
    customHeaderItems: [],
    systemHeaderItems: [],
    defaults: {
        cls: 'promo-detail-panel-default',
        bodyCls: 'promo-detail-panel-default',
        flex: 1,
        margin: '27 0 0 0',
    },
    items: [{
        xtype: 'panel',
        itemId: 'promodetailpanel',
        autoScroll: true,
        cls: 'scrollpanel',
        layout: 'anchor',
        autoRender: true,
        _refreshScroll: this.refreshScroll,
        tpl: Ext.create('App.view.tpm.common.competitorPromoDashboardTpl').formatTpl
    }],

    getBaseModel: function (exactryModel) {
        return Ext.ModelManager.getModel('App.model.tpm.competitorpromo.CompetitorPromo');
    }
});