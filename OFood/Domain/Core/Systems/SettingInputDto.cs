namespace OFood.Domain.Core.Systems
{
    /// <summary>
    /// ������Ϣ����DTO
    /// </summary>
    public class SettingInputDto
    {
        /// <summary>
        /// ��ȡ������ ��������ȫ��
        /// </summary>
        public string SettingTypeName { get; set; }

        /// <summary>
        /// ��ȡ������ ����ģ��JSON
        /// </summary>
        public string SettingJson { get; set; }
    }
}