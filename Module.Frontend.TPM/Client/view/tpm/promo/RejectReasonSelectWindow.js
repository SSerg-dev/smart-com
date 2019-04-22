Ext.define('App.view.tpm.promo.RejectReasonSelectWindow', {
    extend: 'App.view.core.base.BaseModalWindow',
    alias: 'widget.rejectreasonselectwindow',

    autoScroll: true,
    //cls: 'promoform',
    width: 400,
    minWidth: 400,
    resizable: false,
    record: null,
    promowindow: null,

    layout: {
        type: 'vbox',
        align: 'stretch'
    },

    defaults: {
        flex: 1,
        margin: '10 8 15 15'
    },

    title: l10n.ns('tpm', 'text').value('rejectReason'),

    initComponent: function () {
        this.callParent(arguments);
        this.down('textarea[name=comment]').setVisible(false);
    },

    items: [{
        xtype: 'editorform',
        columnsCount: 1,
       
        items: [{
            xtype: 'custompromopanel',
            margin: 2,
            overflowY: 'auto',
            padding: 5,
            layout: {
                type: 'vbox',
                align: 'stretch'
            },
            items: [{
            xtype: 'fieldset',
            title: l10n.ns('tpm', 'RejectReasonWindow').value('Reason'),
            layout: {
                type: 'vbox',
                align: 'stretch',
            },
            defaults: {
                padding: '0 3 0 3',
            },
            items: [{
                    xtype: 'searchfield',
                    //fieldLabel: l10n.ns('tpm', 'Promo').value('Reason'),
                    name: 'RejectReasonId',
                    allowBlank: false,
                    allowOnlyWhitespace: false,
                    selectorWidget: 'rejectreason',
                    valueField: 'Id',
                    displayField: 'Name',
                    store: {
                        type: 'directorystore',
                        model: 'App.model.tpm.rejectreason.RejectReason',
                        extendedFilter: {
                            xclass: 'App.ExtFilterContext',
                            supportedModels: [{
                                xclass: 'App.ExtSelectionFilterModel',
                                model: 'App.model.tpm.rejectreason.RejectReason',
                                modelId: 'efselectionmodel'
                            }]
                        }
                    },
                    listeners: {
                        change: function (field, newValue, oldValue) {
                            var form = field.up('editorform'),
                                commentField = form.down('textarea[name=comment]'),
                                record = this.store.getById(newValue);

                            if (record) {
                                if (record.data.SystemName === 'Other')
                                    commentField.setVisible(true);
                                else
                                    commentField.setVisible(false);
                            }
                        }
                    },
                    onTrigger2Click: function () {
                        var me = this;
                        me.clearValue();

                        var commentField = me.up('editorform').down('textarea[name=comment]');
                        commentField.setVisible(false);
                    },
                    mapping: [{
                        from: 'Name',
                        to: 'RejectReasonName'
                    }]
                }]
            }, {
                xtype: 'fieldset',
                title: l10n.ns('tpm', 'RejectReasonWindow').value('Comment'),
                layout: {
                    type: 'vbox',
                    align: 'stretch',
                },
                defaults: {
                    padding: '0 3 0 3',
                },
                items: [{
                    xtype: 'textarea',
                    name: 'comment',
                    maxLength: 255,
                    height: 110,
                    allowBlank: false,
                    allowOnlyWhitespace: false,
                }]
            }]
        }],
    }],

    buttons: [{
        text: l10n.ns('core', 'buttons').value('cancel'),
        itemId: 'cancel'
    }, {
        text: l10n.ns('core', 'buttons').value('ok'),
        itemId: 'apply',
    }]
})