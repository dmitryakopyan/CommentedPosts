using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using CommentedPostsFront.Models;
using CommentedPostsFront.ViewModels;

namespace CommentedPostsFront
{
	public class MapperProfile : Profile
	{
		public MapperProfile()
		{
			CreateMap<Post, PostViewModel>();
			CreateMap<Comment, CommentViewModel>();
		}
	}
}
