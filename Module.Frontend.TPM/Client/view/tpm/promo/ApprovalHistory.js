Ext.define('App.view.tpm.promo.ApprovalHistory', {
    extend: 'Ext.container.Container',
    alias: 'widget.approvalhistory',

    name: 'changes',
    itemId: 'changes',
    isLoaded: false,
    historyArray: [],
    historyTpl: Ext.create('App.view.tpm.common.approvalHistoryTpl').formatTpl,

    items: [{
        xtype: 'fieldset',
        title: l10n.ns('tpm', 'promoMainTab').value('changeStatusHistory'),
        layout: 'fit',
        cls: 'approval-history-fieldset',
        items: [{
            xtype: 'panel',
            overflowY: 'scroll',
            cls: 'scrollpanel',
            dockedItems: [{
                xtype: 'toolbar',
                dock: 'top',
                items: ['->', {
                    xtype: 'label',
                    text: l10n.ns('tpm', 'text').value('dateOfChange'),
                    cls: 'approval-history-reverse-lable'
                }, {
                    xtype: 'button',
                    text: '',
                    glyph: 0xf4bd,
                    ascendDate: true,
                    cls: 'approval-history-reverse-btn',
                    listeners: {
                        click: function (btn) {
                            var panel = btn.up('panel');
                            var container = panel.up('container[name=changes]');
                            container.historyArray.reverse();

                            var itemsArray = [];
                            for (var i = 0; i < container.historyArray.length; i++) {
                                if (i == container.historyArray.length - 1) {
                                    container.historyArray[i].IsLast = true;
                                } else {
                                    container.historyArray[i].IsLast = false;
                                }

                                itemsArray.push({
                                    html: container.historyTpl.apply(container.historyArray[i]),
                                });
                            }

                            panel.removeAll();
                            panel.add(itemsArray);
                            panel.doLayout();

                            btn.ascendDate = !btn.ascendDate;

                            if (btn.ascendDate) {
                                btn.setGlyph(0xf4bd);
                            } else {
                                btn.setGlyph(0xf4bc);
                            }
                        },
                        scope: this
                    }
                }]
            }],

            items: [{}]
        }]
    }]
});
