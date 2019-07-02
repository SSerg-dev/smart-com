Ext.define('App.view.tpm.promocalculating.CalculatingInfoWindow', {
    extend: 'App.view.core.base.BaseModalWindow',
    alias: 'widget.calculatinginfowindow',
    title: 'Log',//l10n.ns('tpm', 'compositePanelTitles').value('PromoCalculatingWindow'),

    renderTo: Ext.getBody(),
    constrain: true,
    modal: true,
    width: 850,
    minWidth: 850,
    minHeight: 220,
    maxHeight: 580,
    layout: 'fit',

    // id обработчика, лог которого читается
    handlerId: null,

    items: [{
        xtype: 'panel',
        layout: {
            type: 'vbox',
            align: 'stretch'
        },
        items: [{
            xtype: 'custompromopanel',
            height: 130,
            minHeight: 130,
            flex: 1,
            padding: '10 10 10 10',
            items: [{
                xtype: 'fieldset',
                title: 'Info',
                layout: {
                    type: 'vbox',
                    align: 'stretch',
                    pack: 'center',
                },
                defaults: {
                    margin: '5 5 0 5',
                },
                items: [{
                    xtype: 'triggerfield',
                    name: 'Task',
                    editable: false,
                    fieldLabel: 'Task',
                    trigger1Cls: 'form-info-trigger',
                    cls: 'borderedField-with-lable',
                    labelCls: 'borderedField-label',
                    labelWidth: 100,
                    labelSeparator: '',
                    onTrigger1Click: function () {
                        var printPromoCalculatingWin = Ext.create('App.view.tpm.promo.PromoCalculatingWindow');
                        printPromoCalculatingWin.show();

                        var textarea = printPromoCalculatingWin.down('textarea'),
                            infoWindow = this.up('calculatinginfowindow');

                        if (textarea && infoWindow) {
                            textarea.setValue(infoWindow.logText);
                        }
                    },
                    listeners: {
                        afterrender: function (el) {
                            el.addCls('readOnlyField');
                            el.triggerCell.addCls('form-info-trigger-cell');
                        },
                    },
                }, {
                    xtype: 'triggerfield',
                    name: 'Status',
                    editable: false,
                    fieldLabel: 'Status',
                    trigger1Cls: 'emptyField-trigger',
                    labelCls: 'borderedField-label',
                    labelWidth: 100,
                    labelSeparator: '',
                    listeners: {
                        afterrender: function (el) {
                            el.addCls('readOnlyField');
                            el.triggerCell.addCls('form-info-trigger-cell');
                        }
                    },
                }]
            }]
        }, {
            xtype: 'container',
            padding: '0 10 10 10',
            items: [{
                xtype: 'calculatinginfolog',
            }]
        }]
    }],

    buttons: [{
        text: l10n.ns('core', 'buttons').value('download'),
        hidden: true,
        itemId: 'downloadSchedulerFile',
        disabled: true,
        style: { "background-color": "#66BB6A" },
    }, {
        xtype: 'tbspacer',
        flex: 10
    }, {
        text: l10n.ns('tpm', 'button').value('Close'),
        action: 'cancel',
        handler: function () {
            this.up('window').close();
        }
    }]
});