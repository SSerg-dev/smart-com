Ext.define('App.view.tpm.nonpromosupport.NonPromoSupportBrandTech', {
    extend: 'Ext.panel.Panel',
	alias: 'widget.nonpromosupportbrandtech',

    items: [{
        xtype: 'custompromopanel',
        style: 'padding: 0 !important;',
        minHeight: 0,
        margin: 1,
        layout: {
            type: 'vbox',
            align: 'stretch'
		},
		dockedItems: [{
			xtype: 'promosupporttoptoolbar',
			dock: 'top'
		}],
        items: [{
            xtype: 'container',
            padding: '10 10 0 10',
            defaults: {
                margin: 0,
            },
			items: [{
				xtype: 'container',
				flex: 1,
				items: [{
					xtype: 'fieldset',
					title: l10n.ns('tpm', 'NonPromoSupportBrandTech').value('ChooseBrandTech'),
					padding: '1 4 9 4',
					height: 130,
					items: [{
						xtype: 'container',
						layout: {
							type: 'hbox',
							align: 'stretch'
						},
						items: [{
							xtype: 'container',
							padding: '0 5 0 7',
							items: [{
								xtype: 'button',
								itemId: 'chooseBrandTechBtn',
								glyph: 0xf968,
								scale: 'large',
								height: 98,
								width: 110,
								text: '<b>' + l10n.ns('tpm', 'NonPromoSupportBrandTech').value('ChooseBrandTech') + '<br/>...</b>',
								iconAlign: 'top',
								cls: 'custom-event-button promobasic-choose-btn',
								disabledCls: 'promobasic-choose-btn-disabled',
							}]
						}, {
							xtype: 'container',
							flex: 1,
							padding: '0 5 0 7',
							items: [{
								xtype: 'singlelinedisplayfield',
								name: 'Brand',
								width: '100%',
								fieldLabel: l10n.ns('tpm', 'NonPromoSupportBrandTech').value('Brand'),
								allowBlank: false,
								allowOnlyWhiteSpace: false,
							}, {
								xtype: 'singlelinedisplayfield',
								name: 'Technology',
								width: '100%',
								fieldLabel: l10n.ns('tpm', 'NonPromoSupportBrandTech').value('Technology'),
								allowBlank: false,
								allowOnlyWhiteSpace: false,
							}]
						}]
					}]
				}]
			}]
        }]
    }]
});

function onChangeFieldEvent(field) {

    //TODO: требуется доработка при изменении сохраненной записи
    //if (selectedItem) {
    //    selectedItem.saved = false;
    //}

    //createButton = editor.down('#createPromoSupport').setDisabled(true);
    //createOnTheBasisButton = editor.down('#createPromoSupportOnTheBasis').setDisabled(true);
}