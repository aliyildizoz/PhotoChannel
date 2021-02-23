using System;
using System.Collections.Generic;
using System.Text;
using AutoMapper;
using Business.Abstract;

namespace Business.AutoMapperConfig
{
    public class BusinessMapperCfg
    {
        private static MapperConfiguration _cfg;
        private static BusinessMapperCfg businessMapperCfg;
        private static object obj = new object();
        private BusinessMapperCfg(MapperConfiguration cfg)
        {
            _cfg = cfg;
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
                            businessMapperCfg = new BusinessMapperCfg(new MapperConfiguration(expression =>
                                expression.AddProfile(new BusinessProfile())));
                        }
                    }
                }

                return businessMapperCfg;
            }

        }
        public IMapper Mapper()
        {
            return _cfg.CreateMapper();
        }
    }
}
