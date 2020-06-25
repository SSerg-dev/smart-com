﻿Ext.define('App.view.tpm.technology.TechnologyEditor', {
    extend: 'App.view.core.common.EditorDetailWindow',
    alias: 'widget.technologyeditor',
    width: 500,
    minWidth: 500,
    maxHeight: 500,
    cls: 'readOnlyFields',

    listeners: {
        show: function (window) {
            // Workaround для решения бага с прокрукой, если она должная появиться непосредственно при открытии окна
            window.doLayout();

            // скрываем кнопку Edit, если запись открыта из узла дерева
            if (Ext.ComponentQuery.query('producttree').length > 0) {
                window.down('#edit').setVisible(false);
            }
        }
    },

    items: {
        xtype: 'editorform',
        columnsCount: 1,
        items: [{
            xtype: 'textfield',
            name: 'Name',
            fieldLabel: l10n.ns('tpm', 'Technology').value('Name'),
        }, {
            xtype: 'textfield',
            name: 'Tech_code',
            fieldLabel: l10n.ns('tpm', 'Technology').value('Tech_code'),
            regex: /^\d+$/,
            regexText: l10n.ns('tpm', 'Technology').value('DigitRegex')
        }, {
            xtype: 'textfield', allowBlank: true, allowOnlyWhitespace: true,
            name: 'SubBrand',
            fieldLabel: l10n.ns('tpm', 'Technology').value('SubBrand'),
            validator: function (value) {
                var SubCode = this.up('editorform').down('[name = SubBrand_code]');
                if ((!SubCode.getValue() == "" || !SubCode.getValue() == null) && (value == "" || value == null)) {
                    return l10n.ns('tpm', 'Technology').value('ValidateSubBrandRequired');
                }

                if ((SubCode.getValue() == "" || SubCode.getValue() == null) && (value != "" && value != null)) {
                    return l10n.ns('tpm', 'Technology').value('ValidateSubBrandNotRequired');
                }

                return true;
            }
        }, {
            xtype: 'textfield', allowBlank: true, allowOnlyWhitespace: true,
            name: 'SubBrand_code',
            fieldLabel: l10n.ns('tpm', 'Technology').value('SubBrand_code'),
            regex: /^\d+$/,
            regexText: l10n.ns('tpm', 'Technology').value('DigitRegex'),
            listeners: {
                change: function (me) {
                    me.up('editorform').down('[name = SubBrand]').validate();
                }
            }
        }]
    }
});  