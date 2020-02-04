Ext.define('App.view.tpm.nonpromosupport.CustomNonPromoSupportEditor', {
    extend: 'Ext.window.Window',
    alias: 'widget.customnonpromosupporteditor',
    title: l10n.ns('tpm', 'compositePanelTitles').value('NonPromoSupport'),
    ghost: false,
    header: false,
    modal: true,
    constrain: true,
    layout: 'fit',

    height: '90%',
    width: '65%',
    minWidth: 600,
    minHeight: 540,

    items: [{
        xtype: 'panel',
        layout: 'fit',

        defaults: {
            padding: '10 15 15 15'
        },

        dockedItems: [{
            xtype: 'nonpromosupportbottomtoolbar',
            dock: 'bottom'
        },{
            xtype: 'promosupportformtoptoolbar',
            dock: 'top'
        }],

        items: [{
            xtype: 'panel',
            layout: 'fit',
            style: {
                background: '#829cb8',
            },

            dockedItems: [{
                xtype: 'promosupporttoptoolbar',
                dock: 'top'
            }],

            items: [{
                xtype: 'container',
                itemId: 'customPromoSupportEditorContainer',
                flex: 1,
                height: '100%',
                padding: '5 5 0 5',
                cls: 'promo-support-custom-promo-panel-container',
                layout: {
                    type: 'vbox',
                    align: 'stretch'
                },
				items: [{
					xtype: 'nonpromosupportform',
					height: 'auto'
				}, {
					xtype: 'nonpromosupportbrandtech',
                    flex: 1
				}]
			}]
        }]
	}],
    listeners: {
        afterRender: function () {
            var me = this,
                ddConfig;
            var customHeader = me.items.items[0].dockedItems.items[1];
            customHeader.items.items[0].setText(l10n.ns('tpm', 'compositePanelTitles').value('NonPromoSupport'));

            ddConfig = Ext.applyIf({
                el: me.el,
                delegate: '#' + Ext.escapeId(customHeader.id)
            }, me.draggable);

            me.dd = new Ext.util.ComponentDragger(this, ddConfig);
            me.relayEvents(me.dd, ['dragstart', 'drag', 'dragend']);
        }
    }
})