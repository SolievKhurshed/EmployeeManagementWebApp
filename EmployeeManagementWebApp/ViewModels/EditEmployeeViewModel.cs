﻿using EmployeeManagementWebApp.Models;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace EmployeeManagementWebApp.ViewModels
{
    public class EditEmployeeViewModel
    {
        public int Id { get; set; }

        [DisplayName("Имя")]
        [Required(ErrorMessage = "Заполните имя")]
        public string FirstName { get; set; }

        [DisplayName("Фамилия")]
        [Required(ErrorMessage = "Заполните фамилию")]
        public string LastName { get; set; }

        [DisplayName("Отчество")]
        public string MiddleName { get; set; }

        [DisplayName("Учетная запись")]
        [Required(ErrorMessage = "Заполните Учетную запись")]
        public string Login { get; set; }

        [DisplayName("Электронный адрес")]
        [Required(ErrorMessage = "Заполните электронный адрес")]
        public string Email { get; set; }

        [DisplayName("Добавочный номер")]
        [Range(1000, 9999, ErrorMessage = "Укажите внутренний номер от 1000 до 9999.")]
        public int? ExtensionPhone { get; set; }

        [DisplayName("Дата рождения")]
        public DateTime? BirthDate { get; set; }

        [DisplayName("Дата устройства")]
        public DateTime? EmploymentDate { get; set; }

        [DisplayName("Статус")]
        public bool IsActive { get; set; }

        [DisplayName("Группа")]
        [Required(ErrorMessage = "Выберите группу")]
        public string GroupValue { get; set; }
        public Group Group { get; set; }

        [DisplayName("Должность (только чтение)")]
        public string PositionReadOnly { get; set; }

        [DisplayName("Номер мобильного телефона")]        
        public string MobilePhone { get; set; }
    }
}
