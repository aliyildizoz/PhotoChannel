using System;
using System.Collections.Generic;
using System.Text;
using AutoMapper;
using Business.Abstract;

namespace Business.AutoMapperConfig
{
    public class BusinessMapperCfg
    {
        private static MapperConfiguration cfg;
        private static BusinessMapperCfg businessMapperCfg;
        private static object obj = new object();
        private BusinessMapperCfg()
        {

        }
        public static BusinessMapperCfg Instance
        {
            get
            {
                if (businessMapperCfg == null)
                {
                    lock (obj)
                    {
                        if (businessMapperCfg == null)
                        {
                            businessMapperCfg = new BusinessMapperCfg();
                            cfg = new MapperConfiguration(expression => expression.AddProfile(new BusinessProfile()));
                        }
                    }
                }

                return businessMapperCfg;
            }

        }
        public IMapper Mapper()
        {
            return cfg.CreateMapper();
        }
    }
}
