Ext.define('App.view.core.common.PasswordField', {
    extend: 'Ext.form.field.Trigger',
    alias: 'widget.passwordfield',
    inputType: 'password',
    trigger1Cls: Ext.baseCSSPrefix + 'form-password-trigger',
    showPassCls: Ext.baseCSSPrefix + 'form-password-trigger',
    hidePassCls: Ext.baseCSSPrefix + 'form-hide-password-trigger',

    onTriggerClick: function () {
        var type = this.inputEl.dom.type;

        if (type == 'text') {
            this.inputEl.dom.type = 'password';
            this.triggerEl.elements[0].removeCls(this.hidePassCls).addCls(this.showPassCls);
        } else {
            this.inputEl.dom.type = 'text';
            this.triggerEl.elements[0].removeCls(this.showPassCls).addCls(this.hidePassCls);
        }
    }
});