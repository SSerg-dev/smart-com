﻿Ext.define('App.view.tpm.common.approvalHistoryTpl', {
    formatTpl: new Ext.XTemplate([
        '<div style="{[this.isLastBlock(values.IsLast)]} padding: 3px 20px 25px 15px; margin-left: 25px;">',
        '<div class="approval-history-circle"><span style="font-size: 16px; font-family: MaterialDesignIcons; position: absolute; margin: 1px 0px 0px 1px; color: #CFD8DC"></span></div>',
        '<span style="color: #697278; font-size: 12px;">{[this.drawDate(values.Date)]}</span>',
        '<div class="approval-history-box" style="border-color: {StatusColor};">',
        '<span style="text-align:left;"><b>Status: {StatusName}</b></span><br>',
        '{[this.drawRoleAndName(values.RoleName, values.UserName)]}<br>{[this.drawComment(values.MechanicComment)]}',
        '</div>',
        '</div>',
        {
            drawRoleAndName: function (role, name) {
                // если изменения делает система, то RoleName и UserName равны null
                if (role !== null && name !== null)
                    return role + ': ' + name;
                else
                    return 'System';
            },
            drawDate: function (value) {
                return Ext.util.Format.date(value, 'd.m.Y H:i:s');
            },
            drawComment: function (value) {
                return (value == null || value == '') ? '' : 'Comment: ' + value;
            },
            isLastBlock: function (value) {
                return value == true ? 'border-left: none;' : 'border-left: 2px solid #CFD8DC;';
            }
        }
    ])
})