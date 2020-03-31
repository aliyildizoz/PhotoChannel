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

        public static string SubscribeAdded = "Abonelik başarıyla tamamlandı.";
        public static string SubscribeDeleted = "Abonelik başarıyla iptal edildi.";
        public static string ChannelAdminAdded = "Yönetici ekleme başarılı.";
        public static string ChannelAdminDeleted = "Yönetici silme başarılı.";

        public static string UserNotFound = "Kullanıcı bulunamadı";
        public static string PasswordAndUsernameError = "E-mail veya şifre yanlış.";
        public static string PasswordError = "Şifre hatalı";
        public static string SuccessfulLogin = "Sisteme giriş başarılı";
        public static string UserAlreadyExists = "Bu kullanıcı zaten mevcut";
        public static string UserRegistered = "Kullanıcı başarıyla kaydedildi";
        public static string AccessTokenCreated = "Access token başarıyla oluşturuldu";

        public static string UserNotAdded = "Kullanıcı Eklenemedi";
        public static string ChannelNameAlreadyExists = "Kanal adı zaten mevcut.";
        public static string ChannelNotFound = "Kanal bulunamadı.";
        public static string PhotoNotFound = "Fotoğraf bulunamadı.";
        public static string CategoryNotFound = "Kategori bulunamadı.";
    }
}
