Ext.define('App.view.tpm.brand.BrandEditor', {
    extend: 'App.view.core.common.EditorDetailWindow',
    alias: 'widget.brandeditor',
    width: 500,
    minWidth: 500,
    maxHeight: 500,
    cls: 'readOnlyFields',

    items: {
        xtype: 'editorform',
        columnsCount: 1,
        items: [{
            xtype: 'textfield',
            name: 'Name',
            fieldLabel: l10n.ns('tpm', 'Brand').value('Name'),
        }, {
            xtype: 'textfield',
            name: 'Brand_code',
            fieldLabel: l10n.ns('tpm', 'Brand').value('Brand_code'),
            maxLength: 20,
            regex: /^\d+$/,
            regexText: l10n.ns('tpm', 'Brand').value('DigitRegex')
        }, {
            xtype: 'textfield',
            name: 'Segmen_code',
            fieldLabel: l10n.ns('tpm', 'Brand').value('Segmen_code'),
            maxLength: 20,
            regex: /^\d+$/,
            regexText: l10n.ns('tpm', 'Brand').value('DigitRegex')
        }]
    }
});     