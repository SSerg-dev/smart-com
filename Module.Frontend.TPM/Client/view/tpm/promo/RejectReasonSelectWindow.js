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
    title: l10n.ns('tpm', 'text').value('rejectReason'),

    initComponent: function () {
        this.callParent(arguments);

        var commentField = this.down('textarea[name=comment]');
        commentField.setDisabled(true);
        commentField.addCls('readOnlyTextArea');
    },

    items: [{
        xtype: 'editorform',
        columnsCount: 1,

        items: [{
            xtype: 'custompromopanel',  
            widht: '100%',
            height: '100%',
            margin: '0 0 10 0',
            padding: '5 10 5 10',
            overflowY: 'auto',
            
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
                pack: 'center',
            },
            padding: '0 10 10 10',
            defaults: {
                margin: '5 0 0 0',
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
                                if (record.data.SystemName === 'Other') {
                                    commentField.setDisabled(false);
                                    commentField.removeCls('readOnlyTextArea');
                                }
                                else {
                                    commentField.setDisabled(true);
                                    commentField.addCls('readOnlyTextArea');                                    
                                }

                                if (newValue != oldValue)
                                    commentField.reset();
                            }
                        }
                    },
                    onTrigger2Click: function () {
                        var me = this;
                        me.clearValue();

                        var commentField = me.up('editorform').down('textarea[name=comment]');                        
                        commentField.setDisabled(true);
                        commentField.addCls('readOnlyTextArea');
                        commentField.reset();
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
                    pack: 'center',
                },
                padding: '0 10 10 10',
                defaults: {
                    margin: '5 0 0 0',
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