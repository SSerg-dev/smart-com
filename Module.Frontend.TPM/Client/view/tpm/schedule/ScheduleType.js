Ext.define('App.view.tpm.schedule.ScheduleType', {
    extend: 'App.view.core.base.BaseModalWindow',
    alias: 'widget.scheduletypewindow',
    title: l10n.ns('tpm', 'compositePanelTitles').value('AddPromoType'),

    cls: 'promo-support-type',

    minWidth: 500,
    maxWidth: 500,
    minHeight: 345,
    maxHeight: 345,

    resizable: false,
    layout: {
        type: 'vbox',
        align: 'stretch',
        pack: 'center'
    },

    items: [{
        xtype: 'container',
        itemId: 'scheduletypewindowInnerContainer',
        cls: 'custom-promo-panel-container promo-type-select-list-container scrollpanel',
        autoScroll: true,
        hidden: true,
        layout: {
            type: 'vbox',
            align: 'stretch',
        },
        items: [{
            xtype: 'custompromopanel',
            cls: 'custom-promo-panel',
            height: '100%',
            items: [{
                xtype: 'fieldset',
                itemId: 'fieldsetWithButtonsForPromoSupport',
                title: 'Promo Type',
                cls: 'promo-support-type-select-list-fieldset',
                items: []
            }]
        }]
    }]
});