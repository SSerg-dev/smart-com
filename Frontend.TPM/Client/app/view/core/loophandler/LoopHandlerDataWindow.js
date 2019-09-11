Ext.define('App.view.core.loophandler.LoopHandlerDataWindow', {
    extend: 'App.view.core.base.BaseReviewWindow',
    alias: 'widget.loophandlerdatawindow',
    resizeHandles: 'w e',

    title: l10n.ns('core').value('taskDetailsWindowTitle'),

    width: 600,
    height: null,
    minWidth: 600,
    minHeight: 150,
    maxHeight: 500,

    defaults: {
        flex: 0,
        margin: '0 8 15 15'
    },

    items: [{
        xtype: 'simplecombineddirectorypanel',
        title: l10n.ns('core', 'LoopHandler').value('TaskTitle'),
        margin: '10 8 15 15',

        items: [{
            xtype: 'loophandlerparametersform',
            itemId: 'taskform',

            items: [{
                name: 'CreateDate',
                fieldLabel: l10n.ns('core', 'LoopHandler').value('CreateDate'),
                renderer: Ext.util.Format.dateRenderer('d.m.Y H:i:s')
            }, {
                name: 'Description',
                fieldLabel: l10n.ns('core', 'LoopHandler').value('Description')
            }, {
                name: 'Status',
                fieldLabel: l10n.ns('core', 'LoopHandler').value('Status')
            }, {
                xtype: 'fieldcontainer',
                cls: 'labelable-button',
                fieldLabel: l10n.ns('core', 'LoopHandler').value('ReadLogTitle'),

                items: [{
                    xtype: 'button',
                    itemId: 'loophandlerlog',
                    text: l10n.ns('core', 'buttons').value('open')
                }]
            }]
        }]
    }, {
        xtype: 'simplecombineddirectorypanel',
        title: l10n.ns('core', 'LoopHandler').value('ResultTitle'),
        suppressSelection: true,

        items: [{
            xtype: 'loophandlerparametersform',
            itemId: 'resultform',

            items: [{
                xtype: 'label',
                cls: 'label-center',
                text: l10n.ns('core').value('taskEmptyText')
            }]
        }]
    }, {
        xtype: 'simplecombineddirectorypanel',
        title: l10n.ns('core', 'LoopHandler').value('ParametersTitle'),
        suppressSelection: true,

        items: [{
            xtype: 'loophandlerparametersform',
            itemId: 'parametersform',

            items: [{
                xtype: 'label',
                cls: 'label-center',
                text: l10n.ns('core').value('taskEmptyText')
            }]
        }]
    }]
});