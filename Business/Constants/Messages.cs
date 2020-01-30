using System;
using System.Collections.Generic;
using System.Text;

namespace Business.Constants
{
    public static class Messages
    {
        public static string ChannelAdded = "Kanal başarıyla eklendi";
        public static string ChannelDeleted = "Kanal başarıyla silindi";
        public static string ChannelUpdated = "Kanal başarıyla güncellendi";

        public static string SubscribeDeleted = "Abonelik başarıyla tamamlandı.";
        public static string SubscribeAdded = "Abonelik başarıyla iptal edildi.";
        public static string ChannelAdminDeleted = "Yönetici ekleme başarılı.";
        public static string ChannelAdminAdded = "Yönetici silme başarılı.";

        public static string UserNotFound = "Kullanıcı bulunamadı";
        public static string PasswordError = "Şifre hatalı";
        public static string SuccessfulLogin = "Sisteme giriş başarılı";
        public static string UserAlreadyExists = "Bu kullanıcı zaten mevcut";
        public static string UserRegistered = "Kullanıcı başarıyla kaydedildi";
        public static string AccessTokenCreated = "Access token başarıyla oluşturuldu";

        public static string UserNotAdded = "Kullanıcı Eklenemedi";
        public static string ChannelNameAlreadyExists = "Kanal adı zaten mevcut.";
    }
}
